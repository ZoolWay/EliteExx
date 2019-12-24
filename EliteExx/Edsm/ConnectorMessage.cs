using System;

namespace Zw.EliteExx.Edsm
{
    /// <summary>
    /// Actor messages around the EDSM connector.
    /// </summary>
    public abstract class ConnectorMessage : Core.IConnectorMessage
    {
        /// <summary>
        /// Message to initialize the EDSM Connector.
        /// Sent by: ConnectorManager.
        /// Received by: Connector.
        /// Rate: Only once (per Connector start).
        /// </summary>
        public class Init : ConnectorMessage
        {
        }

        /// <summary>
        /// Requests the Elite server state from EDSM.
        /// Sent by: Anyone.
        /// Received by: Connector.
        /// Rate: Unlimited.
        /// </summary>
        public class RequestEliteServerState : ConnectorMessage
        {
        }

        /// <summary>
        /// Requests data about the given system from EDSM.
        /// Sent by: Anyone.
        /// Received by: Connector.
        /// Rate: Unlimited.
        /// </summary>
        public class RequestSystemData : ConnectorMessage
        {
            public string SystemName { get; }

            public RequestSystemData(string systemName)
            {
                SystemName = systemName;
            }
        }
    }
}
