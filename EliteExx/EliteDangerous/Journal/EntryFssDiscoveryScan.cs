using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryFssDiscoveryScan : Entry
    {
        public double Progress { get; }
        public int BodyCount { get; }
        public int NonBodyCount { get; }
        public string SystemName { get; }
        public long SystemAddress { get; }

        public EntryFssDiscoveryScan(DateTime timestamp, Event @event, double progress, int bodyCount, int nonBodyCount, string systemName, long systemAddress) : base(timestamp, @event)
        {
            this.Progress = progress;
            this.BodyCount = bodyCount;
            this.NonBodyCount = nonBodyCount;
            this.SystemName = systemName;
            this.SystemAddress = systemAddress;
        }
    }
}
