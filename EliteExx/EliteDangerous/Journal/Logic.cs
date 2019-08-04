using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public static class Logic
    {
        public static bool IsTerraformable(EntryScanDetailed scan)
        {
            return (String.Compare(scan.TerraformState, "Terraformable", true) == 0);
        }

        public static bool IsWaterworld(EntryScanDetailed scan)
        {
            return (String.Compare(scan.PlanetClass, "Water world", true) == 0);
        }

        public static bool IsEarthlike(EntryScanDetailed scan)
        {
            return (String.Compare(scan.PlanetClass, "Earthlike body") == 0);
        }

        public static bool IsAmmoniaWorld(EntryScanDetailed scan)
        {
            return (String.Compare(scan.PlanetClass, "Ammonia world", true) == 0);
        }

        public static bool IsHighlightedScan(EntryScanDetailed scan)
        {
            return (IsTerraformable(scan) || IsWaterworld(scan) || IsEarthlike(scan) || IsAmmoniaWorld(scan));
        }
    }
}
