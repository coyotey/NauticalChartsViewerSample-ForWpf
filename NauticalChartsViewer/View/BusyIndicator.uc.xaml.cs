using System.Windows;
using System.Windows.Controls;

namespace NauticalChartsViewer
{
    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator : UserControl
    {
        public BusyIndicator()
        {
            InitializeComponent();
        }

        public string  BusyMessage
        {
            get { return (string )GetValue(BusyMessageProperty); }
            set { SetValue(BusyMessageProperty, value); }
        }

        public static readonly DependencyProperty BusyMessageProperty = DependencyProperty.Register("BusyMessage", typeof(string ), typeof(BusyIndicator), new PropertyMetadata("Busy..."));

        
    }
}
