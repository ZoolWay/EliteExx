using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntrySaaScanComplete : Entry
    {
        public EntrySaaScanComplete(DateTime timestamp, Event @event, string bodyName, long bodyID, int probesUsed, int efficiencyTarget) : base(timestamp, @event)
        {
            this.BodyName = bodyName;
            this.BodyID = bodyID;
            this.ProbesUsed = probesUsed;
            this.EfficiencyTarget = efficiencyTarget;
        }

        public string BodyName { get; }
        public long BodyID { get; }
        public int ProbesUsed { get; }
        public int EfficiencyTarget { get; }
    }
}
