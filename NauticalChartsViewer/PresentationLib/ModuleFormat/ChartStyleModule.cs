using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ThinkGeo.MapSuite
{
    public abstract class ChartStyleModule : LibraryFormatModule
    {
        private Size boundingBox;
        private Dictionary<char, string> colorReferences;
        private Collection<string> commands;
        private char commandType;
        private string name;
        private Point pivot;
        private Point upperLeft;

        protected ChartStyleModule()
        {
        }

        public Size BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        public Dictionary<char, string> ColorReferences
        {
            get
            {
                if (colorReferences == null)
                {
                    colorReferences = new Dictionary<char, string>();
                }

                return colorReferences;
            }
        }

        public Collection<string> Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new Collection<string>();
                }

                return commands;
            }
        }

        public virtual char CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Point Pivot
        {
            get { return pivot; }
            set { pivot = value; }
        }

        public Point UpperLeft
        {
            get { return upperLeft; }
            set { upperLeft = value; }
        }
    }
}