using System;

namespace Zw.EliteExx.EliteDangerous
{
    /// <summary>
    /// Actor messages around the Elite Dangerous connector.
    /// </summary>
    public abstract class ConnectorMessage : Core.IConnectorMessage
    {
        /// <summary>
        /// Message to initialize the Elite Dangerous Connector.
        /// Sent by: ConnectorManager.
        /// Received by: Connector.
        /// Rate: Only once (per Connector start).
        /// </summary>
        public class Init : ConnectorMessage
        {
        }

        /// <summary>
        /// Publishes a parsed journal entry.
        /// Sent by: JournalProcessor.
        /// Received by: Connector.
        /// Rate: Unlimited.
        /// </summary>
        public class JournalEntry : ConnectorMessage
        {
            public Journal.Entry Entry { get; }

            public JournalEntry(Journal.Entry entry)
            {
                this.Entry = entry;
            }
        }
    }
}
