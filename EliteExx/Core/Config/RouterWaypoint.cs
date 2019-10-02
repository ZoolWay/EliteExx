using System;

namespace Zw.EliteExx.Core.Config
{
    public class RouterWaypoint
    {
        public int Order { get; }
        public string Name { get; }
        public bool IsDone { get; }

        public RouterWaypoint(int order, string name, bool isDone)
        {
            this.Order = order;
            this.Name = name;
            this.IsDone = isDone;
        }
    }
}
