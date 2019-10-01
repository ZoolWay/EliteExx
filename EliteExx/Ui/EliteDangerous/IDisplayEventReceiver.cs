using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public interface IDisplayEventReceiver
    {
        BindableCollection<DisplayEvent> Events { get; }
        string ShipName { get; set; }
        string PositionStation { get; set; }
        string PositionStarPos { get; set; }
        string PositionSystem { get; set; }
        string PositionSystemBodies { get; set; }
    }
}
