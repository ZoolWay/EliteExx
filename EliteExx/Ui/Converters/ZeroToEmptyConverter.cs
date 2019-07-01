using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.EliteExx.Ui.Converters
{
    public class ZeroToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is char c)
            {
                if (c == '\0') return String.Empty;
                return value;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
