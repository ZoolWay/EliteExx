using System;

namespace Zw.EliteExx.EliteDangerous
{
    /// <summary>
    /// Actor messages around the Elite Dangerous connector.
    /// </summary>
    public abstract class ConnectorMessage
    {
        /// <summary>
        /// Message to initialize the Elite Dangerous Connector.
        /// Sent by: ConnectorManager.
        /// Received by: Connector.
        /// Frequence: Only once (per Connector start).
        /// </summary>
        public class Init : ConnectorMessage
        {
        }
    }
}
