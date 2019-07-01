using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryLocation : Entry
    {
        public EntryLocation(DateTime timestamp, Event @event, bool docked, string starSystem, StarPos starPos, string systemAllegiance, string systemEconomy, string systemGovernment, string systemSecurity, string body, BodyType bodyType) : base(timestamp, @event)
        {
            this.Docked = docked;
            this.StarSystem = starSystem;
            this.StarPos = starPos;
            this.SystemAllegiance = systemAllegiance;
            this.SystemEconomy = systemEconomy;
            this.SystemGovernment = systemGovernment;
            this.SystemSecurity = systemSecurity;
            this.Body = body;
            this.BodyType = bodyType;
        }

        public bool Docked { get; }
        public string StarSystem { get; }
        public StarPos StarPos { get; }
        public string SystemAllegiance { get; }
        public string SystemEconomy { get; }
        public string SystemGovernment { get; }
        public string SystemSecurity { get; }
        public string Body { get; }
        public BodyType BodyType { get; } 
    }
}
