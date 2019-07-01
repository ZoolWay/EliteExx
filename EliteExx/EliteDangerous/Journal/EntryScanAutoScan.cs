using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Zw.EliteExx.Core;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    /// <summary>
    /// Journal entry for a scan with detail level 'auto-scan'.
    /// </summary>
    public class EntryScanAutoScan : Entry
    {
        public ScanType ScanType { get; }
        public string BodyName { get; }
        public long BodyID { get; }
        public ImmutableArray<DynamicBodyIdentifier> Parents { get; }
        public double DistanceFromArrivalLS { get; }
        public bool WasDiscovered { get; }
        public bool WasMapped { get; }

        // STARS
        public string StarType { get; }
        public int? Subclass { get; }
        public double? StellarMass { get; }
        public double? AbsoluteMagnitude { get; }
        public int? Age_MY { get; }
        public string Luminosity { get; }

        // STARS OR BODIES
        public ImmutableArray<Ring> Rings { get; }
        public double? Radius { get; }
        public double? SurfaceTemperature { get; }
        public double? SemiMajorAxis { get; }
        public double? Eccentricity { get; }
        public double? OrbitalInclination { get; }
        public double? Periapsis { get; }
        public double? OrbitalPeriod { get; }
        public double? RotationPeriod { get; }
        public double? AxialTilt { get; }

        public EntryScanAutoScan(DateTime timestamp, Event @event, ScanType scanType, string bodyName, long bodyID, IEnumerable<DynamicBodyIdentifier> parents,
            double distanceFromArrivalLS, bool wasDiscovered, bool wasMapped, string starType, int? subclass, double? stellarMass, double? absoluteMagnitude, int? age_MY,
            double? surfaceTemperature, string luminosity, double? semiMajorAxis, double? eccentricity, double? orbitalInclination, double? periapsis, double? orbitalPeriod,
            double? rotationPeriod, double? axialTilt, double? radius, IEnumerable<Ring> rings)
            : base(timestamp, @event)
        {
            ScanType = scanType;
            BodyName = bodyName;
            BodyID = bodyID;
            Parents = parents.ToImmutableArrayOrEmpty();
            DistanceFromArrivalLS = distanceFromArrivalLS;
            WasDiscovered = wasDiscovered;
            WasMapped = wasMapped;
            StarType = starType;
            Subclass = subclass;
            StellarMass = stellarMass;
            AbsoluteMagnitude = absoluteMagnitude;
            Age_MY = age_MY;
            SurfaceTemperature = surfaceTemperature;
            Luminosity = luminosity;
            SemiMajorAxis = semiMajorAxis;
            Eccentricity = eccentricity;
            OrbitalInclination = orbitalInclination;
            Periapsis = periapsis;
            OrbitalPeriod = orbitalPeriod;
            RotationPeriod = rotationPeriod;
            AxialTilt = axialTilt;
            Radius = radius;
            Rings = rings.ToImmutableArrayOrEmpty();
        }
    }
}
