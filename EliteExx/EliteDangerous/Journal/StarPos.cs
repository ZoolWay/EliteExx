using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    [Newtonsoft.Json.JsonConverter(typeof(StarPosJsonConverter))]
    public class StarPos
    {
        public double? X { get; }
        public double? Y { get; }
        public double? Z { get; }

        public StarPos(double? x, double? y, double? z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
