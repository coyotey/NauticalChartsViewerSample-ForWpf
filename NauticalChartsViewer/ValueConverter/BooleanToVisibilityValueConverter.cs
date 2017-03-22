using System.Windows;
using System.Windows.Data;

namespace NauticalChartsViewer
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool rs = System.Convert.ToBoolean(value);
            return rs ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
