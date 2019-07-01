using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Akka.Actor;
using Newtonsoft.Json.Linq;
using Zw.EliteExx.Core.Config;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Processes files from a directory in sequence of their last write access.
    /// </summary>
    internal class SequenceFilesReaderActor: ReceiveActor
    {
        private readonly Encoding SCANSTATE_ENCODING = Encoding.UTF8;
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly Env env;
        private readonly string id;
        private readonly string directory;
        private readonly string filenamePattern;
        private readonly IActorRef reader;
        private readonly Dictionary<string, DateTime> processedFileTimestamps;

        public SequenceFilesReaderActor(Env env, string id, string directory, string filenamePattern, IActorRef reader)
        {
            this.env = env;
            this.id = id;
            this.directory = directory;
            this.filenamePattern = filenamePattern;
            this.reader = reader;
            this.processedFileTimestamps = new Dictionary<string, DateTime>();

            Receive((Action<ReaderMessage.ScanFiles>)ReceivedScanFiles);
            Receive((Action<ReaderMessage.Processed>)ReceivedProcessed);
        }

        protected override void PostStop()
        {
            SaveScanState();
        }

        private void ReceivedProcessed(ReaderMessage.Processed message)
        {
            var name = Path.GetFileName(message.Name);
            this.processedFileTimestamps[name] = message.ProcessedUntil;
        }

        private void ReceivedScanFiles(ReaderMessage.ScanFiles message)
        {
            LoadScanState();            

            var files = Directory.GetFiles(this.directory, this.filenamePattern, SearchOption.TopDirectoryOnly);
            var filesToProcess = files
                .Select((fn) => new FileInfo(fn))
                .Where((fi) => IsNewOrChanged(fi))
                .OrderBy((fi) => fi.LastWriteTimeUtc)
                .Select((fi) => fi.FullName)
                .ToArray()
                ;
            log.Info("FilesReader-{0}: {1} files found, {2} require processing", this.id, files.Length, filesToProcess.Length);

            this.reader.Tell(new ReaderMessage.QueueFiles(filesToProcess.ToImmutableArray()));
        }

        private bool IsNewOrChanged(FileInfo fi)
        {
            if (!this.processedFileTimestamps.ContainsKey(fi.Name)) return true; // is new
            DateTime lastProcessedWrite = this.processedFileTimestamps[fi.Name];
            return (fi.LastWriteTimeUtc > lastProcessedWrite);
        }

        private string GetScanStateFile()
        {
            return Path.Combine(this.env.AppFolder, $"scanstate.{id}.json");
        }

        private void SaveScanState()
        {
            string scanStateFile = GetScanStateFile();
            JObject rootnode = null;
            if (File.Exists(scanStateFile))
            {
                string contents = File.ReadAllText(scanStateFile, SCANSTATE_ENCODING);
                rootnode = JObject.Parse(contents);
            }
            else
            {
                rootnode = new JObject();
            }
            JObject directoryNode = null;
            if (rootnode.ContainsKey(this.directory))
            {
                var token = rootnode[this.directory];
                if (token.Type != JTokenType.Object) return; // should never happen
                directoryNode = token as JObject;
            }
            else
            {
                directoryNode = new JObject();
                rootnode[this.directory] = directoryNode;
            }
            foreach ((string name, DateTime lastProcessedTs) in this.processedFileTimestamps)
            {
                directoryNode[name] = lastProcessedTs;
            }
            string newContents = rootnode.ToString();
            File.WriteAllText(scanStateFile, newContents, SCANSTATE_ENCODING);
        }

        private void LoadScanState()
        {
            string scanStateFile = GetScanStateFile();
            if (!File.Exists(scanStateFile)) return;
            string contents = File.ReadAllText(scanStateFile, SCANSTATE_ENCODING);
            var rootnode = JObject.Parse(contents);
            if (!rootnode.ContainsKey(this.directory)) return;
            var directoryNode = rootnode[this.directory] as JObject;
            if ((directoryNode == null) || (directoryNode.Type != JTokenType.Object)) return;
            foreach(var x in directoryNode)
            {
                string filename = x.Key;
                this.processedFileTimestamps[filename] = x.Value.Value<DateTime>();
            }
        }
    }
}
