using GalaSoft.MvvmLight.Messaging;

namespace NauticalChartsViewer
{
    internal class MenuItemMessage : MessageBase
    {
        public MenuItemMessage(BaseMenuItem menuItem) 
        {
            MenuItem = menuItem;
        }

        public BaseMenuItem MenuItem { get; private set; }
    }
}
