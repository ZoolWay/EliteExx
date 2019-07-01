using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class StationFaction
    {
        public string Name { get; }
        public string FactionState { get; }

        public StationFaction(string name, string factionState)
        {
            this.Name = name;
            this.FactionState = factionState;
        }
    }
}
