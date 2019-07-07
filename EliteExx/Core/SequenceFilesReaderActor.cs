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
        private FileSystemWatcher fileSystemWatcher;
        private bool firstScanCompleted;
        private ICancelable debouncedSave;

        public SequenceFilesReaderActor(Env env, string id, string directory, string filenamePattern, IActorRef reader)
        {
            this.env = env;
            this.id = id;
            this.directory = directory;
            this.filenamePattern = filenamePattern;
            this.reader = reader;
            this.processedFileTimestamps = new Dictionary<string, DateTime>();
            this.firstScanCompleted = false;

            Receive((Action<ReaderMessage.ScanFiles>)ReceivedScanFiles);
            Receive((Action<ReaderMessage.Processed>)ReceivedProcessed);
            Receive((Action<ReaderMessage.SaveState>)ReceivedSaveState);
        }

        protected override void PreStart()
        {
            this.fileSystemWatcher = new FileSystemWatcher()
            {
                Path = this.directory,
                Filter = this.filenamePattern,
                EnableRaisingEvents = false,
            };
            IActorRef eventReceiver = Self;
            ReaderMessage.ScanFiles eventMessage = new ReaderMessage.ScanFiles();
            this.fileSystemWatcher.Changed += new FileSystemEventHandler((sender, e) => { eventReceiver.Tell(eventMessage); });
        }

        protected override void PostStop()
        {
            this.debouncedSave.CancelIfNotNull();
            this.fileSystemWatcher.EnableRaisingEvents = false;
            this.fileSystemWatcher.Dispose();
            this.fileSystemWatcher = null;
            SaveScanState();
        }

        private void ReceivedProcessed(ReaderMessage.Processed message)
        {
            var name = Path.GetFileName(message.Name);
            this.processedFileTimestamps[name] = message.ProcessedUntil;
            if (this.debouncedSave == null) this.debouncedSave = Context.System.Scheduler.ScheduleTellOnceCancelable(1000, Self, new ReaderMessage.SaveState(), Self);
        }

        private void ReceivedSaveState(ReaderMessage.SaveState message)
        {
            this.debouncedSave = null;
            SaveScanState();
        }

        private void ReceivedScanFiles(ReaderMessage.ScanFiles message)
        {
            if (!this.firstScanCompleted)
            {
                LoadScanState();
                this.fileSystemWatcher.EnableRaisingEvents = true;
            }

            string[] filesFound = Directory.GetFiles(this.directory, this.filenamePattern, SearchOption.TopDirectoryOnly);
            FileInfo[] filesOrdered = filesFound
                .Select((fn) => new FileInfo(fn))
                .OrderBy((fi) => fi.LastWriteTimeUtc)
                .ToArray();
            string[] filesToProcess = filesOrdered
                .Where((fi) => IsNewOrChanged(fi))
                .Select((fi) => fi.FullName)
                .ToArray()
                ;
            if (filesToProcess.Length > 0)
            {
                log.Info("FilesReader-{0}: {1} files found, {2} require processing", this.id, filesFound.Length, filesToProcess.Length);
                this.reader.Tell(new ReaderMessage.QueueFiles(filesToProcess.ToImmutableArray()));
            }
            else
            {
                log.Info("FilesReader-{0}: {1} files found, nothing new, reading last one", this.id, filesFound.Length);
                this.reader.Tell(new ReaderMessage.QueueFiles(ImmutableArray.Create<string>(filesOrdered.Last().FullName)));
            }
            this.firstScanCompleted = true;
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
