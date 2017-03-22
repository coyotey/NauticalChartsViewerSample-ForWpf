using System.Collections.ObjectModel;

namespace NauticalChartsViewer
{
    public class CompositeMenuItem : BaseMenuItem
    {
        private CompositeMenuItem() { }

        public CompositeMenuItem(string header, Collection<object> children)
            : base(header)
        {
            Children = children;
        }

        public Collection<object> Children { get; private set; }
    }
}
