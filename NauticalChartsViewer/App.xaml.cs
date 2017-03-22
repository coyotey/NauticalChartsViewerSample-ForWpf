using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace NauticalChartsViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
            DispatcherHelper.UIDispatcher.UnhandledException += (sender, e) =>
            {
                e.Handled = true;
                MessageBox.Show(e.Exception.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}
