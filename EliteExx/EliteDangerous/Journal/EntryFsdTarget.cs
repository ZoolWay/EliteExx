using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryFsdTarget : Entry
    {
        public string Name { get; }
        public long SystemAddress { get; }

        public EntryFsdTarget(DateTime timestamp, Event @event, string name, long systemAddress) : base(timestamp, @event)
        {
            this.Name = name;
            this.SystemAddress = systemAddress;
        }
    }
}
