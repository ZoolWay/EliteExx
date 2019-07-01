using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryUndocked : Entry
    {
        public EntryUndocked(DateTime timestamp, Event @event, string stationName, string stationType, long marketID) : base(timestamp, @event)
        {
            this.StationName = stationName;
            this.StationType = stationType;
            this.MarketID = marketID;
        }

        public string StationName { get; }
        public string StationType { get; }
        public long MarketID { get; }
    }
}
