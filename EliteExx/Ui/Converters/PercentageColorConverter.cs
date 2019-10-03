using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Zw.EliteExx.Ui.Converters
{
    public class PercentageColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return Binding.DoNothing;
            if (!(values[0] is double)) return Binding.DoNothing;
            if (!(values[1] is double)) return Binding.DoNothing;
            double v = (double)values[0];
            double max = (double)values[1];
            double p = v / max;
            if (p > 0.5) return Colors.LimeGreen;
            return Colors.Red;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
