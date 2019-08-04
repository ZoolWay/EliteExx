using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    internal class SystemSummaryBuilder
    {
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
                this.receiver.SystemRows.Add(new SystemSummaryRow()
                {
                    Order = 0,
                    Description = $"{ds.SystemName} ({ds.BodyCount} bodies)",
                    BodyType = BodyType.Star,
                    DoneState = DoneState.Done,
                    DataOrigin = Core.DataOrigin.EliteJournal,
                });
            }
            else if (entry is EntryScanDetailed sd)
            {
                var row = CreateOrGetRow(sd);
                row.ExtraInfo = $"class={sd.PlanetClass}, discovered={sd.WasDiscovered}, mapped={sd.WasMapped}";
                row.IsHighlighted = Logic.IsTerraformable(sd) || Logic.IsWaterworld(sd) || Logic.IsEarthlike(sd);
                row.DataOrigin |= Core.DataOrigin.EliteJournal;
            }
            else if (entry is EntryScanAutoScan sas)
            {
                var row = CreateOrGetRow(sas);
                row.DataOrigin |= Core.DataOrigin.EliteJournal;
            }
            else if (entry is EntrySaaScanComplete ssc)
            {
                var row = this.receiver.SystemRows.FirstOrDefault(r => r.BodyId == ssc.BodyID);
                if (row == null) return;
                row.DoneState = DoneState.Done;
            }
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
                //match.BodyType = BodyType.
                this.receiver.SystemRows.Add(match);
            }
            // strip systemname if possible
            if ((match.Description.StartsWith(this.currentSystem)) && (match.Description.Length > this.currentSystem.Length)) match.Description = "   " + match.Description.Substring(this.currentSystem.Length + 1);
            return match;
        }
    }
}
