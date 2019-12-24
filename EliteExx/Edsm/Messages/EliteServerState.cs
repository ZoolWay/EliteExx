using System;

namespace Zw.EliteExx.Edsm.Messages
{
    public class EliteServerState
    {
        public DateTime LastUpdate { get; }
        public string Message { get; }

        public EliteServerState(DateTime lastUpdate, string message)
        {
            this.LastUpdate = lastUpdate;
            this.Message = message;
        }
    }
}
