using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class MaterialComponent
    {
        public string Name { get; }
        public double Percent { get; }

        public MaterialComponent(string name, double percent)
        {
            this.Name = name;
            this.Percent = percent;
        }
    }
}
