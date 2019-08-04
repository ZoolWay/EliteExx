using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zw.EliteExx.Core;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    internal class DisplayEventBuilder
    {
        private readonly IDisplayEventReceiver receiver;

        public DisplayEventBuilder(IDisplayEventReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void CreateDisplayEventFor(Entry entry)
        {
            if (entry is EntryFsdJump j)
            {
                CreateDisplayEventForJump(j);
            }
            else if (entry is EntryDocked docked)
            {
                CreateDisplayEventForDocked(docked);
            }
            else if (entry is EntryUndocked ud)
            {
                CreateDisplayEventForUndocked(ud);
            }
            else if (entry is EntryScanDetailed ds)
            {
                CreateDisplayEventForScanDetailed(ds);
            }
            else if (entry is EntryScanAutoScan @as)
            {
                CreateDisplayEventForScanAuto(@as);
            }
            else if (entry is EntryFssAllBodiesFound fabf)
            {
                CreateDisplayEventForFssAllBodiesFound(fabf);
            }
            else if (entry is EntryFssDiscoveryScan fds)
            {
                CreateDisplayEventForFssDiscoveryScan(fds);
            }
            else if (entry is EntryFileheader fh)
            {
                CreateDisplayEventForFileheader(fh);
            }
            else if (entry is EntryLocation l)
            {
                CreateDisplayEventForLocation(l);
            }
            else if (entry is EntryLoadGame lg)
            {
                CreateDisplayEventForLoadGame(lg);
            }
            else if (entry is EntryLoadout lo)
            {
                CreateDisplayEventForLoadout(lo);
            }
            else if (entry is EntryMetaMessage mm)
            {
                CreateDisplayEventForMetaMessage(mm);
            }
        }

        private void CreateDisplayEventForFileheader(EntryFileheader fh)
        {
            this.receiver.Events.Add(new DisplayEvent()
            {
                Text = $"Fileheader, gameversion {fh.Gameversion}",
                EventType = DisplayEventType.GameStart,
                IsBoring = true,
            });
        }

        private void CreateDisplayEventForFssDiscoveryScan(EntryFssDiscoveryScan fds)
        {
            this.receiver.Events.Add(new DisplayEvent()
            {
                Text = $"{fds.Progress * 100}% {fds.BodyCount} bodies, {fds.NonBodyCount} non-bodies",
                EventType = DisplayEventType.Scan,
                IsBoring = true,
            });
        }

        private void CreateDisplayEventForFssAllBodiesFound(EntryFssAllBodiesFound fabf)
        {
            this.receiver.Events.Add(new DisplayEvent()
            {
                Text = $"{fabf.Count} bodies in {fabf.SystemName}",
                EventType = DisplayEventType.Scan,
            });
        }

        private void CreateDisplayEventForScanAuto(EntryScanAutoScan @as)
        {
            DisplayEvent de = new DisplayEvent()
            {
                Text = $"Auto-Scanned {@as.BodyName} {@as.WasDiscovered} {@as.WasMapped}",
                EventType = DisplayEventType.Scan,
            };
            if (@as.WasDiscovered == false)
            {
                de.IsHighlighted = true;
                de.Symbol2 = '\xf890'; // sparkles
                de.Symbol2Tooltip = "undiscovered!";
            }
            else
            {
                de.IsBoring = true;
            }
            this.receiver.Events.Add(de);
        }

        private void CreateDisplayEventForScanDetailed(EntryScanDetailed ds)
        {
            DisplayEvent de = new DisplayEvent()
            {
                Text = $"Scanned {ds.BodyName} ({ds.PlanetClass}) {ds.WasDiscovered} {ds.WasMapped} {ds.TerraformState} {ds.Landable}",
                EventType = DisplayEventType.Scan,
            };
            if (ds.WasDiscovered == false)
            {
                de.IsHighlighted = true;
                de.Symbol2 = '\xf890'; // sparkles
                de.Symbol2Tooltip = "undiscovered!";
            }
            if (Logic.IsTerraformable(ds))
            {
                de.IsHighlighted = true;
                de.Symbol1 = '\xf7a2'; // globe-europe
                de.Symbol1Tooltip = "terraformable!";
            }
            if (Logic.IsWaterworld(ds))
            {
                de.IsHighlighted = true;
                de.Symbol1 = '\xf7a2';
                de.Symbol1Tooltip = "water world";
                if (de.Symbol2.IsDefaultOrWhitespace())
                {
                    de.Symbol2 = '\xf773'; // water
                    de.Symbol2Tooltip = "water world";
                }
            }
            else if (Logic.IsEarthlike(ds))
            {
                de.IsHighlighted = true;
                de.Symbol1 = '\xf7a2';
                de.Symbol1Tooltip = "earth-like";
                if (de.Symbol2.IsDefaultOrWhitespace())
                {
                    de.Symbol2 = '\xf6bb'; // campground
                    de.Symbol2Tooltip = "earth-like";
                }
            }
            de.IsBoring = !de.IsHighlighted;
            this.receiver.Events.Add(de);
        }

        private void CreateDisplayEventForUndocked(EntryUndocked ud)
        {
            this.receiver.PositionStation = String.Empty;
            DisplayEvent de = new DisplayEvent()
            {
                Text = $"Undocked from {ud.StationName}",
                EventType = DisplayEventType.ShipPiloting,
                Symbol1 = '\xf5b0', // plane-departure
                Symbol1Tooltip = "undocked",
            };
            this.receiver.Events.Add(de);
        }

        private void CreateDisplayEventForDocked(EntryDocked docked)
        {
            this.receiver.PositionStation = docked.StationName;
            DisplayEvent de = new DisplayEvent()
            {
                Text = $"Docked at {docked.StationName} ({docked.StationType})",
                EventType = DisplayEventType.ShipPiloting,
                Symbol1 = '\xf5af', // plane-arrival
                Symbol1Tooltip = "docked",
            };
            this.receiver.Events.Add(de);
        }

        private void CreateDisplayEventForJump(EntryFsdJump j)
        {
            this.receiver.PositionSystem = j.StarSystem;
            this.receiver.PositionStarPos = GetCombinedStarPos(j.StarPos);
            DisplayEvent de = new DisplayEvent()
            {
                Text = $"Jumped to {j.StarSystem} ({j.JumpDist}ly dist, {j.FuelUsed}t fuel)",
                EventType = DisplayEventType.ShipPiloting,
                Symbol1 = '\xf6b0', // alicorn
                Symbol1Tooltip = "jump",
            };
            this.receiver.Events.Add(de);
        }

        private void CreateDisplayEventForLocation(EntryLocation l)
        {
            this.receiver.PositionSystem = l.StarSystem;
            this.receiver.PositionStarPos = GetCombinedStarPos(l.StarPos);
            DisplayEvent de = new DisplayEvent()
            {
                Text = $"Location: {l.StarSystem}",
                EventType = DisplayEventType.ShipPiloting,
                Symbol1 = '\xf3c5', // map-marker-alt
            };
            this.receiver.Events.Add(de);
        }

        private void CreateDisplayEventForLoadout(EntryLoadout lo)
        {
            this.receiver.ShipName = lo.ShipName;
        }

        private void CreateDisplayEventForMetaMessage(EntryMetaMessage mm)
        {
            this.receiver.Events.Add(new DisplayEvent()
            {
                Text = mm.Message,
                EventType = DisplayEventType.GenericEvent,
                Symbol1 = '\xf05a', // info-circle
            });
        }

        private void CreateDisplayEventForLoadGame(EntryLoadGame lg)
        {
            this.receiver.ShipName = lg.ShipName;
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
