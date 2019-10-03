using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous.Position
{
    public interface IPositionReceiver
    {
        BindableCollection<SystemSummaryRow> SystemRows { get; }
        string PositionStation { get; set; }
        string PositionStarPos { get; set; }
        string PositionSystem { get; set; }
        string PositionSystemBodies { get; set; }
    }
}
