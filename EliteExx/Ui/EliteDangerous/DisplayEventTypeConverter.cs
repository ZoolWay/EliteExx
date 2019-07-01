using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class DisplayEventTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DisplayEvent de)
            {
                if (de.EventType == DisplayEventType.Scan) return "\xf7c0"; // satellite-dish
                return "\xf564"; // cookie-bite
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
