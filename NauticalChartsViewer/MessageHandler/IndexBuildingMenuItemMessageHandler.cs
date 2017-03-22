using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class IndexBuildingMenuItemMessageHandler : MenuItemMessageHandler
    {
        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            var window = new BuildingIndexWindow();
            window.Owner = owner;
            window.ShowDialog();
        }

        public override string[] Actions
        {
            get { return new[] { "buildindex" }; }
        }
    }
}
