using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Akka.Actor;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Generic ND-JSON File Reader.
    /// Reads all queued files line-by-line and sends them to a processor. Files must only be queued by one providing actor.
    /// </summary>
    internal class NdJsonFileReaderActor : ReceiveActor
    {
        private const int BUFFER_SIZE = 32 * 1024;
        private static readonly Encoding NDJSON_FILE_ENCODING = Encoding.UTF8;

        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef processor;
        private readonly Queue<string> queuedFiles;
        private readonly Dictionary<string, long> filePositions;
        private string currentFile;
        private DateTime currentFileLastWrite;
        private Stream stream;
        private byte[] buffer;
        private int bufferpos;
        private ICancelable scheduledRead;
        private IActorRef reportQueueProcessTo;

        public NdJsonFileReaderActor(IActorRef processor)
        {
            this.processor = processor;
            this.queuedFiles = new Queue<string>();
            this.filePositions = new Dictionary<string, long>();
            this.buffer = new byte[BUFFER_SIZE];
            this.scheduledRead = null;
            this.reportQueueProcessTo = ActorRefs.Nobody;

            Receive((Action<ReaderMessage.Read>)ReceivedRead);
            Receive((Action<ReaderMessage.QueueFiles>)ReceivedQueueFiles);
        }

        protected override void PostStop()
        {
            if (stream != null) CloseStream();
            scheduledRead.CancelIfNotNull();
        }

        private void ReceivedRead(ReaderMessage.Read message)
        {
            if (stream == null)
            {
                if (this.queuedFiles.Count <= 0) return;
                this.currentFile = this.queuedFiles.Dequeue();
                this.stream = File.Open(this.currentFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            FileInfo fileInfo = new FileInfo(this.currentFile);
            this.currentFileLastWrite = fileInfo.LastWriteTimeUtc;

            int bytesRead = this.stream.Read(this.buffer, this.bufferpos, this.buffer.Length - this.bufferpos);
            this.bufferpos += bytesRead;
            int newLinePos = -1;
            long runLineIndex = 0;

            // extract all new lines and send to the processor
            while ((newLinePos = Array.IndexOf<byte>(this.buffer, (byte)'\n', 0, this.bufferpos)) >= 0)
            {
                string line = NDJSON_FILE_ENCODING.GetString(this.buffer, 0, newLinePos);
                this.processor.Tell(new ReaderMessage.ProcessLine(fileInfo.Name, runLineIndex, line));
                int count = this.bufferpos - newLinePos - 1;
                Array.Copy(this.buffer, newLinePos + 1, this.buffer, 0, count);
                this.bufferpos -= newLinePos + 1;
                runLineIndex++;

                newLinePos = Array.IndexOf<byte>(this.buffer, (byte)'\n', 0, this.bufferpos);
            }

            if (this.bufferpos >= (this.buffer.Length - 1)) throw new Exception("Buffer full before new line was detected!");

            if (!this.reportQueueProcessTo.IsNobody()) this.reportQueueProcessTo.Tell(new ReaderMessage.Processed(this.currentFile, this.currentFileLastWrite));

            if (this.queuedFiles.Count > 0) // more files available, guess we are done with the current one
            {
                CloseStream();
                Self.Tell(new ReaderMessage.Read());
            }
            else // no more files, we should try to continue the current file a little later...
            {
                Context.System.Scheduler.ScheduleTellOnceCancelable(1000, Self, new ReaderMessage.Read(), Self);
            }
        }

        private void ReceivedQueueFiles(ReaderMessage.QueueFiles message)
        {
            if (this.reportQueueProcessTo.IsNobody()) this.reportQueueProcessTo = Sender;
            foreach (var filename in message.FilesToQueue)
            {
                if (String.Equals(this.queuedFiles.LastOrDefault(), filename)) continue; // do not queue if that already at the end of the queue
                this.queuedFiles.Enqueue(filename);
            }
            log.Info($"Queued {message.FilesToQueue.Length} new files, current queue size is now {this.queuedFiles.Count}");
            if (this.queuedFiles.Count > 0) Self.Tell(new ReaderMessage.Read()); // if something in queue, start reading
        }

        private void CloseStream()
        {
            if (stream == null) return;
            stream.Close();
            stream.Dispose();
            stream = null;
            this.bufferpos = 0;
            this.currentFile = null;
            this.currentFileLastWrite = default;
        }
    }
}
