using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zw.EliteExx.Edsm.Responses
{
    public class AtmosphereComposition
    {
        public double? Hydrogen { get; set; }
        public double? Helium { get; set; }
        public double? Oxygen { get; set; }
        public double? Nitrogen { get; set; }
        public double? Ammonia { get; set; }
        public double? Methane { get; set; }
        [JsonProperty(PropertyName = "Carbon dioxide")]
        public double? CarbonDioxide { get; set; }
        [JsonProperty(PropertyName = "Sulphur dioxide")]
        public double? SulphurDioxide { get; set; }
        public double? Argon { get; set; }
        public double? Neon { get; set; }
    }
}
