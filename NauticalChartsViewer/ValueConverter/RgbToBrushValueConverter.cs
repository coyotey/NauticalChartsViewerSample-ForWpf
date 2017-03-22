using System;
using System.Windows.Data;
using System.Windows.Media;

namespace NauticalChartsViewer
{
    [ValueConversion(typeof(ColorItem), typeof(Brush))]
    public class RgbToBrushValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var rgb = (ColorItem)value;
                return new SolidColorBrush(Color.FromRgb((byte)rgb.R, (byte)rgb.G, (byte)rgb.B));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
