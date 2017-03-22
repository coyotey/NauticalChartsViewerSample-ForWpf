using System;
using System.Globalization;
using System.Windows.Data;

namespace NauticalChartsViewer
{
    [ValueConversion(typeof(string), typeof(double))]
    public class StringToDoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return value.ToString();
            }
            catch (Exception)
            {
                return "0";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return double.Parse((string)value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
