using System;
using System.Globalization;
using System.Windows.Data;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.Ui.EliteDangerous.Position
{
    public class SystemSummaryBodyTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is BodyType)) return Binding.DoNothing;
            var bt = (BodyType)value;
            if (bt == BodyType.Star) return '\xf005'; // star
            if (bt == BodyType.Planet) return '\xf111'; // circle
            if (bt == BodyType.PlanetaryRing) return '\xf70b'; // ring
            if (bt == BodyType.Station) return '\xf1b2'; // cube
            if (bt == BodyType.BeltCluster) return '\xf753'; // meteor
            return "\xf059"; // question-circle
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
