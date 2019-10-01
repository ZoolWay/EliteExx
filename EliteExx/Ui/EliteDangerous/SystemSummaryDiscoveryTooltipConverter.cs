using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class SystemSummaryDiscoveryTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SystemSummaryRow)) return Binding.DoNothing;
            SystemSummaryRow row = (SystemSummaryRow)value;
            return $"discovered={row.IsDiscovered}, mapped={row.IsMapped}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
