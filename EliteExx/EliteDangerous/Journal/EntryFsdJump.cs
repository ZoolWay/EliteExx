using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryFsdJump : Entry
    {
        public EntryFsdJump(DateTime timestamp, Event @event, string starSystem, long systemAddress, StarPos starPos, string systemAllegiance, string systemEconomy, string systemSecondEconomy,
            string systemGovernment, string systemSecurity, long population, string body, long bodyID, string bodyType, double jumpDist, double fuelUsed, double fuelLevel)
            : base(timestamp, @event)
        {
            this.StarSystem = starSystem;
            this.SystemAddress = systemAddress;
            this.StarPos = starPos;
            this.SystemAllegiance = systemAllegiance;
            this.SystemEconomy = systemEconomy;
            this.SystemSecondEconomy = systemSecondEconomy;
            this.SystemGovernment = systemGovernment;
            this.SystemSecurity = systemSecurity;
            this.Population = population;
            this.Body = body;
            this.BodyID = bodyID;
            this.BodyType = bodyType;
            this.JumpDist = jumpDist;
            this.FuelUsed = fuelUsed;
            this.FuelLevel = fuelLevel;
        }

        public string StarSystem { get; }
        public long SystemAddress { get; }
        public StarPos StarPos { get; }
        public string SystemAllegiance { get; }
        public string SystemEconomy { get; }
        public string SystemSecondEconomy { get; }
        public string SystemGovernment { get; }
        public string SystemSecurity { get; }
        public long Population { get; }
        public string Body { get; }
        public long BodyID { get; }
        public string BodyType { get; }
        public double JumpDist { get; }
        public double FuelUsed { get; }
        public double FuelLevel { get; }
    }
}
