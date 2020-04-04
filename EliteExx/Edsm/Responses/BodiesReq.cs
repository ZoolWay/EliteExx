using System;

namespace Zw.EliteExx.Edsm.Responses
{
    public class BodiesReq
    {
        public int Id { get; set; }
        public long Id64 { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? BodyCount { get; set; } // nowadays this is often NULL
        public Body[] Bodies { get; set; }
    }
}
