using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace NauticalChartsViewer
{
    [ValueConversion(typeof(Dictionary<string, string>), typeof(string))]
    public class DictionaryToStringValueConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dics = value as Dictionary<string, string>;
            StringBuilder builder = new StringBuilder();

            int i = 0;
            foreach (string key in dics.Keys)
            {
                i++;
                if (!string.IsNullOrEmpty(dics[key]))
                {
                    builder.AppendFormat("{0}:{1}", key, dics[key]);
                    if (i != dics.Keys.Count)
                    {
                        builder.AppendLine();
                    }
                }
            }

            return builder.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
