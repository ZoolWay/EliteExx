using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Zw.EliteExx.Core.Config
{
    public class RouterSettings
    {
        public ImmutableArray<RouterWaypoint> Waypoints { get; }
        public bool HideDone { get; }

        public RouterSettings(IEnumerable<RouterWaypoint> waypoints, bool hideDone)
        {
            this.Waypoints = waypoints.ToImmutableArray();
            this.HideDone = hideDone;
        }
    }
}
