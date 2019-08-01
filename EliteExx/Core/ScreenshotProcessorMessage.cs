using System;

namespace Zw.EliteExx.Core
{
    public abstract class ScreenshotProcessorMessage
    {
        /// <summary>
        /// Message to initialize the ScreenshotProcessor.
        /// Sent by: ActorSystemManager.
        /// Received by: ScreenshotProcessor.
        /// Rate: Only once.
        /// </summary>
        public class Init : ScreenshotProcessorMessage
        {
        }

        /// <summary>
        /// Message to notify about an updated configuration.
        /// Sent by: ActorSystemManager.
        /// Received by: ScreenshotProcessor.
        /// Rate: Unlimited, on on every config change.
        /// </summary>
        public class ConfigUpdated : ConnectorManagerMessage
        {
            public Core.Config.Config NewConfig { get; }

            public ConfigUpdated(Core.Config.Config newConfig)
            {
                this.NewConfig = newConfig;
            }
        }
    }
}
