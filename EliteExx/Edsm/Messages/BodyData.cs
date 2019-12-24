using System;

namespace Zw.EliteExx.Edsm.Messages
{
    public class BodyData
    {
        public long Id64 { get; }
        public string Name { get; }
        public int BodyId { get; }
        public string DiscoveredBy { get; }
        public string TerraformingState { get; }

        public BodyData(long id64, string name, int bodyId, string discoveryBy, string terraformingState)
        {
            this.Id64 = id64;
            this.Name = name;
            this.BodyId = bodyId;
            this.DiscoveredBy = discoveryBy;
            this.TerraformingState = terraformingState;
        }
    }
}
