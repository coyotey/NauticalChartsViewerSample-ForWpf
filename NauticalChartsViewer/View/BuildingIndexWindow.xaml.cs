using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NauticalChartsViewer
{
    /// <summary>
    /// Interaction logic for BuildingIndexWindow.xaml
    /// </summary>
    public partial class BuildingIndexWindow : Window
    {
        private BuildingIndexViewModel buildingIndexViewModel;

        public BuildingIndexWindow()
        {
            InitializeComponent();
            buildingIndexViewModel = new BuildingIndexViewModel();
            DataContext = buildingIndexViewModel;
            Loaded += (sender, e) =>
            {
                Messenger.Default.Register<WindowStateMessage>(this, "BuildIndexWindow", HandleWindowStateMessage);
            };

            Unloaded += (sender, e) =>
            {
                Messenger.Default.Unregister<WindowStateMessage>(this, "BuildIndexWindow");
                buildingIndexViewModel.Cleanup();
            };
        }

        private void HandleWindowStateMessage(WindowStateMessage message)
        {
            switch (message.WindowState)
            {
                case S57WindowState.Close:
                    this.Close();
                    break;
            }
        }
    }
}
