using System.Windows.Data;

namespace NauticalChartsViewer
{
    [ValueConversion(typeof(string), typeof(string))]
    internal class StringToNATextValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = System.Convert.ToString(value);
            if (!string.IsNullOrEmpty(str))
                return str;
            return "N/A";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
