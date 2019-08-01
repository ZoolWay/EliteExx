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
    }
}
