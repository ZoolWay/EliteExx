using System;
using System.Linq;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    internal class SystemSummaryBuilder
    {
        private const string BELT_CLUSTER_ID = "Belt Cluster";
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISystemSummaryReceiver receiver;
        private string currentSystem;

        public SystemSummaryBuilder(ISystemSummaryReceiver receiver)
        {
            this.receiver = receiver;
            this.currentSystem = String.Empty;
        }

        public void Process(Entry entry)
        {
            if (entry is EntryLocation l)
            {
                this.currentSystem = l.StarSystem;
            }
            else if (entry is EntryFsdJump j)
            {
                this.receiver.SystemRows.Clear();
                this.currentSystem = j.StarSystem;
            }
            else if (entry is EntryFssDiscoveryScan ds)
            {
                // no creation of an entry, we already have a header!
            }
            else if (entry is EntryScanDetailed sd)
            {
                var row = CreateOrGetRow(sd);
                row.ExtraInfo = ComposeExtraInfo(row, sd);
                row.IsHighlighted = Logic.IsHighlightedScan(sd);
                row.DataOrigin |= Core.DataOrigin.EliteJournal;
                row.IsDiscovered = true;
            }
            else if (entry is EntryScanAutoScan sas)
            {
                var row = CreateOrGetRow(sas);
                row.ExtraInfo = ComposeExtraInfo(row, sas);
                row.DataOrigin |= Core.DataOrigin.EliteJournal;
            }
            else if (entry is EntrySaaScanComplete ssc)
            {
                var row = this.receiver.SystemRows.FirstOrDefault(r => r.BodyId == ssc.BodyID);
                if (row == null) return;
                row.DoneState = DoneState.Done;
                row.IsDiscovered = true;
                row.IsMapped = true;
            }
        }

        private string ComposeExtraInfo(SystemSummaryRow row, EntryScanAutoScan sas)
        {
            if (row.BodyType == BodyType.Star) return $"Startype: {sas.StarType}";

            EntryScanDetailed sd = sas as EntryScanDetailed;
            if (sd != null)
            {
                if (!String.IsNullOrWhiteSpace(sd.PlanetClass)) return $"{sd.PlanetClass}";
            }

            if (row.Description.Contains(BELT_CLUSTER_ID))
            {
                int idx = row.Description.IndexOf(BELT_CLUSTER_ID);
                return row.Description.Substring(idx);
            }

            return row.ExtraInfo;
        }

        private SystemSummaryRow CreateOrGetRow(EntryScanAutoScan scan)
        {
            var match = this.receiver.SystemRows.FirstOrDefault(r => r.BodyId == scan.BodyID);
            if (match == null)
            {
                match = new SystemSummaryRow();
                match.Order = 1000 + scan.BodyID;
                match.BodyId = scan.BodyID;
                match.Description = scan.BodyName;
                match.ExtraInfo = scan.ScanType.ToString();
                match.BodyType = DetectBodyType(scan);
                match.IsDiscovered = scan.WasDiscovered;
                match.IsMapped = scan.WasMapped;
                this.receiver.SystemRows.Add(match);
            }

            // restyle systemname if possible
            string name = match.Description;
            if (String.Equals(this.currentSystem, name)) name = "Star"; // rename solo star 
            else if ((name.StartsWith(this.currentSystem)) && (name.Length > this.currentSystem.Length)) name = "   " + name.Substring(this.currentSystem.Length + 1); // strip systemname for other bodies
            match.Description = name;

            return match;
        }

        private BodyType DetectBodyType(EntryScanAutoScan scan)
        {
            if (!String.IsNullOrWhiteSpace(scan.StarType)) return BodyType.Star;
            var det = (scan as EntryScanDetailed);
            if (det != null)
            {
                if (!String.IsNullOrWhiteSpace(det.PlanetClass)) return BodyType.Planet;
            }
            return BodyType.Unknown;
        }
    }
}
