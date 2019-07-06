using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class DisplayEventTypeTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string p = parameter as string;
            if (value is DisplayEvent de)
            {
                if (p == "Symbol1") return GetSymbol1Text(de);
                if (p == "Symbol2") return GetSymbol2Text(de);
                return GetRootText(de);
            }
            return Binding.DoNothing;
        }

        private string GetSymbol2Text(DisplayEvent de)
        {
            return de.Symbol2Tooltip;
        }

        private string GetSymbol1Text(DisplayEvent de)
        {
            return de.Symbol1Tooltip;
        }

        private string GetRootText(DisplayEvent de)
        {
            return de.EventType.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
