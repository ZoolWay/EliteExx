using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Actor messages used by directory and file readers.
    /// </summary>
    public abstract class ReaderMessage
    {
        /// <summary>
        /// Orders a directory scanner to scan for files to process.
        /// Sent by: connectors, module connectors (EliteDangerous.Journal), directory scanners.
        /// Received by: directory scanners (SequenceFilesReader).
        /// Rate: Unlimited.
        /// </summary>
        public class ScanFiles : ReaderMessage
        {
        }

        /// <summary>
        /// Saves the current state. Primarly used to debounce saves after receiving Processed-messages.
        /// Sent by: SequenceFilesReaderActor.
        /// Received by: SequenceFilesReaderActor.
        /// Rate: Unlimited.
        /// </summary>
        public class SaveState : ReaderMessage
        {
        }

        /// <summary>
        /// Orders a file reader to read from its queue.
        /// Sent by: file readers.
        /// Received by: file readers (NdJsonFileReader).
        /// Rate: Unlimited.
        /// </summary>
        public class Read : ReaderMessage
        {
        }

        /// <summary>
        /// Queues files a directory scanner found for a file reader.
        /// Sent by: directory scanners (SequenceFilesReader).
        /// Received by: file readers (NdJsonFileReader).
        /// Rate: Unlimited.
        /// </summary>
        public class QueueFiles : ReaderMessage
        {
            public ImmutableArray<string> FilesToQueue { get; }

            public QueueFiles(ImmutableArray<string> filesToQueue)
            {
                this.FilesToQueue = filesToQueue;
            }
        }

        /// <summary>
        /// Notifies a directory scanner about the file processing state.
        /// Sent by: file readers (NdJsonFileReader).
        /// Received by: directory scanners (SequenceFilesReader).
        /// Rate: Unlimited.
        /// </summary>
        [DebuggerDisplay("Processed '{Name}' until {ProcessedUntil}")]
        public class Processed : ReaderMessage
        {
            public string Name { get; }
            public DateTime ProcessedUntil { get; }

            public Processed(string name, DateTime processedUntil)
            {
                this.Name = name;
                this.ProcessedUntil = processedUntil;
            }
        }

        /// <summary>
        /// Sends a read line to a processor.
        /// Sent by: file readers (NdJsonFileReader).
        /// Received by: processors (JournalProcessor).
        /// Rate: Unlimited.
        /// </summary>
        public class ProcessLine : ReaderMessage
        {
            public string SourceFile { get; }
            public long Index { get; }
            public string Line { get; }

            public ProcessLine(string sourceFile, long index, string line)
            {
                this.SourceFile = sourceFile;
                this.Index = index;
                this.Line = line;
            }
        }

        /// <summary>
        /// Notifies a connector about a processing error.
        /// Sent by: processors (JournalProcessor).
        /// Received by: connectors (EliteDangerous.Connector).
        /// Rate: Unlimited.
        /// </summary>
        public class ProcessError : ReaderMessage
        {
            public string Line { get; }
            public string ErrorMessage { get; }

            public ProcessError(string line, string errorMessage)
            {
                this.Line = line;
                this.ErrorMessage = errorMessage;
            }
        }

        /// <summary>
        /// Notifies the directory scanner that its FileSystemWatcher detected an event.
        /// Sent by: directory scanners (SequenceFilesReader).
        /// Received by: directory scanners (SequenceFilesReader).
        /// Rate: Unlimited.
        /// </summary>
        public class NotifyFilesystemEvent : ReaderMessage
        { 
            public string AffectedFullPath { get; }
            public string AffectedName { get; }
            public System.IO.WatcherChangeTypes ChangeType { get; }

            public NotifyFilesystemEvent(string affectedFullPath, string affectedName, System.IO.WatcherChangeTypes changeType)
            {
                this.AffectedFullPath = affectedFullPath;
                this.AffectedName = affectedName;
                this.ChangeType = changeType;
            }
        }
    }
}
