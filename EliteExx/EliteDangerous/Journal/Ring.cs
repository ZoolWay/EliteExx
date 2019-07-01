using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    /// <summary>
    /// Specifies a ring (star or body).
    /// </summary>
    public class Ring
    {
        public string Name { get; }
        public string RingClass { get; }
        public double MassMT { get; }
        public double InnerRad { get; }
        public double OuterRad { get; }

        public Ring(string name, string ringClass, double massMT, double innerRad, double outerRad)
        {
            Name = name;
            RingClass = ringClass;
            MassMT = massMT;
            InnerRad = innerRad;
            OuterRad = outerRad;
        }
    }
}
