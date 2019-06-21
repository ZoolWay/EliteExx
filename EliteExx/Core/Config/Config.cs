using System;

namespace Zw.EliteExx.Core.Config
{
    public class Config
    {
        public Locations Locations { get; }

        public Config(Locations locations)
        {
            this.Locations = locations;
        }
    }
}
