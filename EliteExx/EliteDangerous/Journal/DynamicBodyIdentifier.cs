using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    [System.Diagnostics.DebuggerDisplay("DynBodyId Null={Null}, Star={Star}, Planet={Planet}, Ring={Ring}")]
    public class DynamicBodyIdentifier
    {
        public long? Planet { get; }
        public long? Star { get; }
        public long? Null { get; }
        public long? Ring { get; }

        public DynamicBodyIdentifier(long? planet, long? star, long? @null, long? ring)
        {
            this.Planet = planet;
            this.Star = star;
            this.Null = @null;
            this.Ring = ring;
        }
    }
}
