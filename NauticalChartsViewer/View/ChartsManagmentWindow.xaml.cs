using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace NauticalChartsViewer
{
    /// <summary>
    /// Interaction logic for ChartsManagmentWindow.xaml
    /// </summary>
    public partial class ChartsManagmentWindow : Window
    {
        private readonly ChartsManagmentViewModel chartsManagmentViewModel;
        public ChartsManagmentWindow()
        {
            InitializeComponent();
            chartsManagmentViewModel = ChartsManagmentViewModel.Instance;
            DataContext = chartsManagmentViewModel;
            Loaded += (s, e) =>
            {
                Messenger.Default.Register<WindowStateMessage>(this, HandleChartWindowMessage);
            };

            Unloaded += (sender, e) =>
            {
                Messenger.Default.Unregister<WindowStateMessage>(this, HandleChartWindowMessage);
            };
        }

        private void HandleChartWindowMessage(WindowStateMessage message)
        {
            switch (message.WindowState)
            {
                case S57WindowState.Close:
                    Close();
                    break;
            }
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (object addedItem in e.AddedItems)
            {
                ChartItem chartItem = addedItem as ChartItem;
                if (!chartsManagmentViewModel.SelectedItems.Contains(chartItem))
                {
                    chartsManagmentViewModel.SelectedItems.Add(chartItem);
                }
            }
            foreach (object removedItem in e.RemovedItems)
            {
                ChartItem chartItem = removedItem as ChartItem;
                chartsManagmentViewModel.SelectedItems.Remove(chartItem);
            }
        }
    }
}
