using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.EliteExx.Edsm.Responses
{
    public class Body
    {
        public int Id { get; set; }
        public long Id64 { get; set; }
        public int BodyId { get; set; }
        public string Name { get; set; }
        public Discovery Discovery { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public StarRef[] Parents { get; set; }
        public int DistanceToArrival { get; set; }

        public bool? IsMainStar { get; set; }
        public bool? IsScoopable { get; set; }
        public int? Age { get; set; }
        public string SpectralClass { get; set; }
        public string Luminosity { get; set; }
        public double? AbsoluteMagnitude { get; set; }
        public double? SolarMasses { get; set; }
        public double? SolarRadius { get; set; }

        public bool IsLandable { get; set; }
        public double Gravity { get; set; }
        public double EarthMasses { get; set; }
        public double Radius { get; set; }
        public double SurfacePressure { get; set; }
        public string VolcanismType { get; set; }
        public string AtmopshereType { get; set; }
        // AtmosphereComposition
        public SolidComposition SolidComposition { get; set; }
        public string TerraformingState { get; set; }
        public string ReserveLevel { get; set; }

        public int SurfaceTemperature { get; set; }
        public double? OrbitalPeriod { get; set; }
        public double? SemiMajorAxis { get; set; }
        public double? OrbitalEccentricity { get; set; }
        public double? OrbitalInclination { get; set; }
        public double? ArgOfPeriapsis { get; set; }
        public double RotationalPeriod { get; set; }
        public bool RotationalPeriodTidallyLocked { get; set; }
        public double? AxialTilt { get; set; }
        public Materials Materials { get; set; }
        public Belt[] Belts { get; set; }
        public Ring[] Rings { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
