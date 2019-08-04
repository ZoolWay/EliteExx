using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class DoneStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DoneState)) return Binding.DoNothing;
            var d = (DoneState)value;
            if (d == DoneState.NotDone) return String.Empty;
            if (d == DoneState.Done) return "\xf00c"; // check
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
