using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryFssAllBodiesFound : Entry
    {
        public string SystemName { get; }
        public long SystemAddress { get; }
        public int Count { get; }

        public EntryFssAllBodiesFound(DateTime timestamp, Event @event, string systemName, long systemAddress, int count) : base(timestamp, @event)
        {
            this.SystemName = systemName;
            this.SystemAddress = systemAddress;
            this.Count = count;
        }
    }
}
