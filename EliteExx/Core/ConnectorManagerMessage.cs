﻿using System;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Actor messages around the ConnectorManager.
    /// </summary>
    public abstract class ConnectorManagerMessage
    {
        /// <summary>
        /// Message to initialize the ConnectorManager.
        /// Sent by: ActorSystemManager.
        /// Received by: ConnectorManager.
        /// Frequence: Only once (per ConnectorManager start).
        /// </summary>
        public class Init : ConnectorManagerMessage
        {
        }

        /// <summary>
        /// Message to notify about an updated configuration.
        /// Sent by: Configuration, ConnectorManager.
        /// Received by: ConnectorManager, connector actors.
        /// Frequence: Unlimited, one on every config change.
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
