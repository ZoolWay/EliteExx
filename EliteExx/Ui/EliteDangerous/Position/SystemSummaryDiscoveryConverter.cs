using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.EliteExx.Ui.EliteDangerous.Position
{
    public class SystemSummaryDiscoveryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SystemSummaryRow)) return Binding.DoNothing;
            SystemSummaryRow row = (SystemSummaryRow)value;
            if (row.IsDiscovered && row.IsMapped) return '\xf59f'; // map-marked
            if (row.IsDiscovered) return '\xf606'; // map-marker-check
            return '\xf60b'; // map-marker-question
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
