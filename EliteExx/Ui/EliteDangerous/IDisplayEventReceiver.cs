using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public interface IDisplayEventReceiver
    {
        BindableCollection<DisplayEvent> Events { get; }
        string ShipName { get; set; }
        string Ship { get; set; }
        string ShipIdent { get; set; }
        double FuelCapacity { get; set; }
        double FuelLevel { get; set; }
    }
}
