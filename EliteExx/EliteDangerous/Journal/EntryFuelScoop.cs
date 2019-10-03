using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryFuelScoop : Entry
    {
        public EntryFuelScoop(DateTime timestamp, Event @event, double scooped, double total) : base(timestamp, @event)
        {
            this.Scooped = scooped;
            this.Total = total;
        }

        public double Scooped { get; }
        public double Total { get; }
    }
}
