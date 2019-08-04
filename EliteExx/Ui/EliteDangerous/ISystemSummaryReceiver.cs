using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public interface ISystemSummaryReceiver
    {
        BindableCollection<SystemSummaryRow> SystemRows { get; }
    }
}
