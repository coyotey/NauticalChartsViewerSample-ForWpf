using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class S52Object
    {
        private string name;
        private Collection<DAIShape> shapes;
        private Vertex pivotPoint;
        private int width;
        private int height;
        private Vertex upperLeftPoint;
        private S52ObjectType objectType;

        public S52Object()
        {
           
        }

        public S52ObjectType ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Collection<DAIShape> Shapes
        {
            get { return shapes; }
            set { shapes = value; }
        }

        public Vertex PivotVertex
        {
            get { return pivotPoint; }
            set { pivotPoint = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public Vertex UpperLeftVertex
        {
            get { return upperLeftPoint; }
            set { upperLeftPoint = value; }
        }
    }
}
