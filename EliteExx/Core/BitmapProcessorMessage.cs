using System;

namespace Zw.EliteExx.Core
{
    public abstract class BitmapProcessorMessage
    {
        /// <summary>
        /// Message to initialize the BitmapProcessor.
        /// Sent by: ActorSystemManager.
        /// Received by: BitmapProcessor.
        /// Rate: Only once.
        /// </summary>
        public class Init : BitmapProcessorMessage
        {
        }

        /// <summary>
        /// Message to notify about an updated configuration.
        /// Sent by: ActorSystemManager.
        /// Received by: BitmapProcessor.
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
