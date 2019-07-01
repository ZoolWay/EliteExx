using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class StationEconomy
    {
        public string Name { get; }
        public double  Proportion { get; }

        public StationEconomy(string name, double proportion)
        {
            this.Name = name;
            this.Proportion = proportion;
        }
    }
}
