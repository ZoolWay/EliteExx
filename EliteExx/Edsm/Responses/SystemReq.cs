using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.EliteExx.Edsm.Responses
{
    public class SystemReq
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public long Id64 { get; set; }
        public Coords Coords { get; set; }
        public bool CoordLocked { get; set; }
        public bool RequirePermit { get; set; }
        public Star PrimaryStar { get; set; }
    }
}
