namespace ThinkGeo.MapSuite
{
    internal class PointShape : DAIShape
    {
        private Vertex vertex;

        public PointShape()
            : this(0, 0, 1, new RGBColor())
        {

        }

        public PointShape(int x, int y)
            : this(x, y, 1, new RGBColor())
        {

        }

        public PointShape(Vertex vertex)
            : this(vertex.X, vertex.Y, 1, new RGBColor())
        {

        }

        public PointShape(Vertex vertex, int width, RGBColor color)
            : this(vertex.X, vertex.Y, width, color)
        {

        }

        public PointShape(int x, int y, int width, RGBColor color)
            : base(DAIShapeType.Point, width, color)
        {
            vertex = new Vertex(x, y);
        }

        public int X
        {
            get { return vertex.X; }
            set { vertex.X = value; }
        }

        public int Y
        {
            get { return vertex.Y; }
            set { vertex.Y = value; }
        }
    }
}
