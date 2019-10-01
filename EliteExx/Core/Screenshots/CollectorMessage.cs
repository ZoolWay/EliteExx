using System;

namespace Zw.EliteExx.Core.Screenshots
{
    public abstract class CollectorMessage
    {
        /// <summary>
        /// Message to start a collection from the source folders.
        /// Sent by: Collector.
        /// Received by: Collector.
        /// Rate: Unlimited.
        /// </summary>
        public class Collect : CollectorMessage
        {
        }

        /// <summary>
        /// Message to initialize Collector.
        /// Sent by: ScreenshotProcessor.
        /// Received by: Collector.
        /// Rate: Once.
        /// </summary>
        public class Init : CollectorMessage
        {
        }

        /// <summary>
        /// Message to tell the processor that a file was successfully moved.
        /// Sent by: Collector.
        /// Received by: ScreenshotProcessor.
        /// Rate: Unlimited.
        /// </summary>
        public class Success : CollectorMessage
        {
            public string MovedFilename { get; }

            public Success(string movedFilename)
            {
                this.MovedFilename = movedFilename;
            }
        }
    }
}
