using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class OpacityMenuItemMessageHandler : MenuItemMessageHandler
    {
        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            Messenger.Default.Send(message, "ShowOpacityPanel");
        }

        public override string[] Actions
        {
            get { return new[] { "changechartsopacity" }; }
        }
    }
}
