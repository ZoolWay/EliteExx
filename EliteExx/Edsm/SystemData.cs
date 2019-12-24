using System;
using System.Collections.Immutable;

namespace Zw.EliteExx.Edsm
{
    public class SystemData
    {
        public string Name { get; }
        public ImmutableArray<BodyData> BodyData { get; }

        public SystemData(string name, ImmutableArray<BodyData> bodyData)
        {
            this.Name = name;
            this.BodyData = bodyData;
        }
    }
}
