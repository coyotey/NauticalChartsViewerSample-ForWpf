
namespace NauticalChartsViewer
{
    public class SingleMenuItem : BaseMenuItem
    {
        private SingleMenuItem() { }

        public SingleMenuItem(string icon, string header, string action, string groupName, bool isCheckEnabled, bool isChecked, bool canToggle, CompositeMenuItem parent)
            : base(icon, header, action, groupName, isCheckEnabled, isChecked, canToggle, parent)
        {

        }

    }
}
