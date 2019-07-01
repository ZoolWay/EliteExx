using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryDocked : Entry
    {
        public string StationName { get; }
        public string StationType { get; }
        public string StarSystem { get; }
        public long SystemAddress { get; }
        public long MarketID { get; }
        [JsonConverter(typeof(StationFactionConverter))]
        public StationFaction StationFaction { get; }
        public string StationGovernment { get; }
        public ImmutableArray<StationService> StationServices { get; }
        public string StationEconomy { get; }
        public ImmutableArray<StationEconomy> StationEconomies { get; }
        public double DistFromStarLS { get; }

        public EntryDocked(DateTime timestamp, Event @event, string stationName, string stationType, string starSystem, long systemAddress, long marketID, StationFaction stationFaction, string stationGovernment, IEnumerable<StationService> stationServices, string stationEconomy, IEnumerable<StationEconomy> stationEconomies, double distFromStarLS) : base(timestamp, @event)
        {
            this.StationName = stationName;
            this.StationType = stationType;
            this.StarSystem = starSystem;
            this.SystemAddress = systemAddress;
            this.MarketID = marketID;
            this.StationFaction = stationFaction;
            this.StationGovernment = stationGovernment;
            this.StationServices = stationServices.ToImmutableArray();
            this.StationEconomy = stationEconomy;
            this.StationEconomies = stationEconomies.ToImmutableArray();
            this.DistFromStarLS = distFromStarLS;
        }
    }
}
