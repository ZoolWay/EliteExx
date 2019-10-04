using System;

namespace Zw.EliteExx.Core.Config
{
    public class RouterWaypoint
    {
        public int Order { get; }
        public string Name { get; }
        public bool IsDone { get; }
        public string Notes { get; }

        public RouterWaypoint(int order, string name, bool isDone, string notes)
        {
            this.Order = order;
            this.Name = name;
            this.IsDone = isDone;
            this.Notes = notes;
        }
    }
}
