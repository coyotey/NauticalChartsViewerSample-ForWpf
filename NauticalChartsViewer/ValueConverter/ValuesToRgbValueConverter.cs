using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace NauticalChartsViewer
{
    public class ValuesToRgbValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null)
            {
                if (values[0] != DependencyProperty.UnsetValue &&
                    values[1] != DependencyProperty.UnsetValue &&
                    values[2] != DependencyProperty.UnsetValue)
                {
                    short r = (short)values[0];
                    short g = (short)values[1];
                    short b = (short)values[2];
                    return new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
