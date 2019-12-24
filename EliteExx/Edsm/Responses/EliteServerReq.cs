using System;

namespace Zw.EliteExx.Edsm.Responses
{
    public class EliteServerReq
    {
        public DateTime LastUpdate { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
