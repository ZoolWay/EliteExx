using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Zw.EliteExx.Core;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    /// <summary>
    /// Journal entry for a scan with detail level 'detailed' (FSS).
    /// </summary>
    public class EntryScanDetailed : EntryScanAutoScan
    {
        public bool TidalLock { get; }
        public string TerraformState { get; }
        public string PlanetClass { get; }
        public string Atmosphere { get; }
        public string AtmosphereType { get; }
        public string Volcanism { get; }
        public double MassEM { get; }
        public double SurfaceGravity { get; }
        public ImmutableArray<AtmosphereComponent> AtmosphereComposition { get; }
        public double SurfacePressure { get; }
        public bool Landable { get; }
        public ImmutableArray<MaterialComponent> Materials { get; }
        public Composition Composition { get; }

        public EntryScanDetailed(DateTime timestamp, Event @event, ScanType scanType, string bodyName, long bodyID, IEnumerable<DynamicBodyIdentifier> parents, double distanceFromArrivalLS,
            bool wasDiscovered, bool wasMapped, string starType, int? subclass, double? stellarMass, double? absoluteMagnitude, int? age_MY, double? surfaceTemperature, string luminosity,
            double? semiMajorAxis, double? eccentricity, double? orbitalInclination, double? periapsis, double? orbitalPeriod, double? rotationPeriod, double? axialTilt, double? radius,
            IEnumerable<Ring> rings, bool tidalLock, string terraformState, string planetClass, string atmosphere, string atmosphereType, string volcanism, double massEM, double surfaceGravity,
            IEnumerable<AtmosphereComponent> atmosphereComposition, double surfacePressure, bool landable, IEnumerable<MaterialComponent> materials, Composition composition)
            : base(timestamp, @event, scanType, bodyName, bodyID, parents, distanceFromArrivalLS, wasDiscovered, wasMapped, starType, subclass, stellarMass, absoluteMagnitude, age_MY,
                surfaceTemperature, luminosity, semiMajorAxis, eccentricity, orbitalInclination, periapsis, orbitalPeriod, rotationPeriod, axialTilt, radius, rings)
        {
            TidalLock = tidalLock;
            TerraformState = terraformState;
            PlanetClass = planetClass;
            Atmosphere = atmosphere;
            AtmosphereType = atmosphereType;
            Volcanism = volcanism;
            MassEM = massEM;
            SurfaceGravity = surfaceGravity;
            AtmosphereComposition = atmosphereComposition.ToImmutableArrayOrEmpty();
            SurfacePressure = surfacePressure;
            Landable = landable;
            Materials = materials.ToImmutableArrayOrEmpty();
            Composition = composition;
        }
    }
}
