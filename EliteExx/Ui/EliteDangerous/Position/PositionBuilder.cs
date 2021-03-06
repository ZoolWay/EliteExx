﻿using System;
using System.Linq;
using Zw.EliteExx.Edsm.Messages;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous.Position
{
    internal class PositionBuilder
    {
        private const string BELT_CLUSTER_ID = "Belt Cluster";
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPositionReceiver receiver;
        private readonly Core.ActorSystemManager actorSystemManager;
        private string currentSystem;

        public PositionBuilder(IPositionReceiver receiver, Core.ActorSystemManager asm)
        {
            this.receiver = receiver;
            this.actorSystemManager = asm;
            this.currentSystem = String.Empty;
        }

        public void Process(SystemData systemData)
        {
            if (!String.Equals(this.currentSystem, systemData.Name))
            {
                log.Warn($"Received update from EDSM for '{systemData.Name}' but current system is '{this.currentSystem}'");
                return;
            }
            foreach (var b in systemData.BodyData)
            {
                var r = CreateOrGetRow(b);
                r.DataOrigin |= Core.DataOrigin.Edsm;
            }
        }

        public void Process(Entry entry)
        {
            bool locationChanged = false;
            if (entry is EntryLocation l)
            {
                locationChanged = (!String.Equals(this.currentSystem, l.StarSystem));
                this.currentSystem = l.StarSystem;
                this.receiver.PositionSystem = l.StarSystem;
                this.receiver.PositionStarPos = GetCombinedStarPos(l.StarPos);
            }
            else if (entry is EntryFsdJump j)
            {
                locationChanged = (!String.Equals(this.currentSystem, j.StarSystem));
                this.receiver.SystemRows.Clear();
                this.currentSystem = j.StarSystem;
                this.receiver.PositionSystem = j.StarSystem;
                this.receiver.PositionStarPos = GetCombinedStarPos(j.StarPos);
            }
            else if (entry is EntryFssDiscoveryScan ds)
            {
                this.receiver.PositionSystemBodies = $"({ds.BodyCount} bodies)";
            }
            else if (entry is EntryFssAllBodiesFound abf)
            {
                this.receiver.PositionSystemBodies = $"({abf.Count} bodies)";
            }
            else if (entry is EntryScanDetailed sd)
            {
                var row = CreateOrGetRow(sd);
                row.ExtraInfo = ComposeExtraInfo(row, sd);
                row.IsHighlighted = Logic.IsHighlightedScan(sd);
                row.DataOrigin |= Core.DataOrigin.EliteJournal;
                row.IsDiscovered = true;
                row.IsPlaceholder = false;
            }
            else if (entry is EntryScanAutoScan sas)
            {
                var row = CreateOrGetRow(sas);
                row.ExtraInfo = ComposeExtraInfo(row, sas);
                row.DataOrigin |= Core.DataOrigin.EliteJournal;
                row.IsPlaceholder = false;
            }
            else if (entry is EntrySaaScanComplete ssc)
            {
                var row = this.receiver.SystemRows.FirstOrDefault(r => r.BodyId == ssc.BodyID);
                if (row == null) return;
                row.DoneState = DoneState.Done;
                row.IsDiscovered = true;
                row.IsMapped = true;
                row.IsPlaceholder = false;
            }
            else if (entry is EntryUndocked u)
            {
                this.receiver.PositionStation = String.Empty;
            }
            else if (entry is EntryDocked d)
            {
                this.receiver.PositionStation = d.StationName;
            }

            if (locationChanged)
            {
                this.actorSystemManager.Tell(new Edsm.ConnectorMessage.RequestSystemData(this.currentSystem));
            }
        }

        private string ComposeExtraInfo(SystemSummaryRow row, EntryScanAutoScan sas)
        {
            if (row.BodyType == BodyType.Star)
            {
                if (sas.StarType == "N") return "Neutron star";
                return $"Startype: {sas.StarType}";
            }

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

        private SystemSummaryRow CreateOrGetRow(BodyData bd)
        {
            var match = this.receiver.SystemRows.FirstOrDefault(r => r.BodyId == bd.BodyId);
            if (match == null)
            {
                match = new SystemSummaryRow();
                match.Order = 1000 + bd.BodyId;
                match.BodyId = bd.BodyId;
                match.Description = bd.Name;

                match.DataOrigin = Core.DataOrigin.Edsm;
                match.IsPlaceholder = true;
                match.IsDiscovered = !String.IsNullOrWhiteSpace(bd.DiscoveredBy);

                this.receiver.SystemRows.Add(match);
            }

            match.Description = RestyleName(match.Description);

            return match;
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
            else if (match.DataOrigin == Core.DataOrigin.Edsm) // was only known from EDSM before...
            {
                match.ExtraInfo = scan.ScanType.ToString();
                match.BodyType = DetectBodyType(scan);
                match.IsDiscovered = scan.WasDiscovered;
                match.IsMapped = scan.WasMapped;
            }

            match.Description = RestyleName(match.Description);

            return match;
        }

        private string RestyleName(string currentName)
        {
            // restyle systemname if possible
            if (String.Equals(this.currentSystem, currentName)) currentName = "Star"; // rename solo star 
            else if ((currentName.StartsWith(this.currentSystem)) && (currentName.Length > this.currentSystem.Length)) currentName = "   " + currentName.Substring(this.currentSystem.Length + 1); // strip systemname for other bodies
            return currentName;
        }

        private BodyType DetectBodyType(EntryScanAutoScan scan)
        {
            if (!String.IsNullOrWhiteSpace(scan.StarType)) return BodyType.Star;
            if (scan.BodyName.Contains(BELT_CLUSTER_ID)) return BodyType.BeltCluster;
            var det = (scan as EntryScanDetailed);
            if (det != null)
            {
                if (!String.IsNullOrWhiteSpace(det.PlanetClass)) return BodyType.Planet;
            }
            return BodyType.Unknown;
        }

        private string GetCombinedStarPos(StarPos starPos)
        {
            return $"({GetStarPos(starPos)})";
        }

        private string GetStarPos(StarPos starPos)
        {
            return String.Format("{0}/{1}/{2}", starPos.X, starPos.Y, starPos.Z);
        }
    }
}
