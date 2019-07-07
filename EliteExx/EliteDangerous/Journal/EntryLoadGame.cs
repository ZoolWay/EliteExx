using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryLoadGame : Entry
    {
        public string FID { get; }
        public string Commander { get; }
        public bool Horizons { get; }
        public string Ship { get; }
        public int ShipID { get; }
        public string ShipName { get; }
        public string ShipIdent { get; }
        public double FuelLevel { get; }
        public double FuelCapacity { get; }
        public string GameMode { get; }
        public long Credits { get; }
        public long Loan { get; }

        public EntryLoadGame(DateTime timestamp, Event @event, string fID, string commander, bool horizons, string ship, int shipID, string shipName, string shipIdent, double fuelLevel, double fuelCapacity, string gameMode, long credits, long loan) : base(timestamp, @event)
        {
            this.FID = fID;
            this.Commander = commander;
            this.Horizons = horizons;
            this.Ship = ship;
            this.ShipID = shipID;
            this.ShipName = shipName;
            this.ShipIdent = shipIdent;
            this.FuelLevel = fuelLevel;
            this.FuelCapacity = fuelCapacity;
            this.GameMode = gameMode;
            this.Credits = credits;
            this.Loan = loan;
        }
    }
}
