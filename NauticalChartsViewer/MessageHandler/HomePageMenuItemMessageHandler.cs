using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class HomePageMenuItemMessageHandler : MenuItemMessageHandler
    {
        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://wiki.thinkgeo.com/"); 
        }

        public override string[] Actions
        {
            get { return new[] { "thinkgeohomepage" }; }
        }
    }
}
