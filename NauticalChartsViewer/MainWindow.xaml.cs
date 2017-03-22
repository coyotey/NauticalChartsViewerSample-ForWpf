using System.Globalization;
using System.Windows;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => mainViewModel.Cleanup();
        }

        private void WpfMap_CurrentScaleChanging(object sender, CurrentScaleChangingWpfMapEventArgs e)
        {
            CurrentScale.Text = string.Format("1:{0:N}", e.CurrentScale);
        }

        private void WpfMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point currentPoint = e.GetPosition(WpfMap);
            PointShape worldPoint = ExtentHelper.ToWorldCoordinate(WpfMap.CurrentExtent, new ScreenPointF((float)currentPoint.X, (float)currentPoint.Y), (float)WpfMap.ActualWidth, (float)WpfMap.ActualHeight);

            CurrentX.Text = worldPoint.X.ToString("f6", CultureInfo.InvariantCulture);
            CurrentY.Text = worldPoint.Y.ToString("f6", CultureInfo.InvariantCulture);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainViewModel = new MainViewModel(WpfMap);
            DataContext = mainViewModel;
        }
    }
}