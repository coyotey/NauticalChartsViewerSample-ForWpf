using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class LineShape : DAIShape
    {
        private Collection<Vertex> vertexes;

        public LineShape()
            : this(new Collection<Vertex>(), 1, new RGBColor())
        {

        }

        public LineShape(Collection<Vertex> vertexes)
            : this(vertexes, 1, new RGBColor())
        {

        }

        public LineShape(Collection<Vertex> vertexes, int width, RGBColor color)
            : base(DAIShapeType.Line, width, color)
        {
            this.vertexes = vertexes;
        }

        public Collection<Vertex> Vertexes
        {
            get { return vertexes; }
            set { vertexes = value; }
        }
    }
}
