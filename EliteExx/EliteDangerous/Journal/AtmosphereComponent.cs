using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class AtmosphereComponent
    {
        public string Name { get; }
        public double Percent { get; }

        public AtmosphereComponent(string name, double percent)
        {
            this.Name = name;
            this.Percent = percent;
        }
    }
}
