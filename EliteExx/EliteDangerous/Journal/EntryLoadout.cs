using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryLoadout : Entry
    {
        public string Ship { get; }
        public int ShipID { get; }
        public string ShipName { get; }
        public string ShipIdent { get; }
        public long HullValue { get; }
        public long ModulesValue { get; }
        public double HullHealth { get; }
        public double UnladenMass { get; }
        public int CargoCapacity { get; }
        public double MaxJumpRange { get; }

        // FuelCapacity
        // Rebuy
        // Modules

        public EntryLoadout(DateTime timestamp, Event @event, string ship, int shipID, string shipName, string shipIdent, long hullValue, long modulesValue, double hullHealth, double unladenMass, int cargoCapacity, double maxJumpRange) : base(timestamp, @event)
        {
            this.Ship = ship;
            this.ShipID = shipID;
            this.ShipName = shipName;
            this.ShipIdent = shipIdent;
            this.HullValue = hullValue;
            this.ModulesValue = modulesValue;
            this.HullHealth = hullHealth;
            this.UnladenMass = unladenMass;
            this.CargoCapacity = cargoCapacity;
            this.MaxJumpRange = maxJumpRange;
        }
    }
}
