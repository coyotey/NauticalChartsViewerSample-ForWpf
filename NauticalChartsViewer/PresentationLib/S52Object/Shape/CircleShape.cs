namespace ThinkGeo.MapSuite
{
    internal class CircleShape : DAIShape
    {
        private int radius;
        private Vertex center;

        public CircleShape()
            : this(new Vertex(), 0, 1, new RGBColor())
        {

        }

        public CircleShape(Vertex center, int radius)
            : this(center, radius, 1, new RGBColor())
        {

        }

        public CircleShape(Vertex center, int radius, int width, RGBColor color)
            : base(DAIShapeType.Circle, width, color)
        {
            this.center = center;
            this.radius = radius;
        }

        public Vertex Center
        {
            get { return center; }
            set { center = value; }
        }

        public int Raduis
        {
            get { return radius; }
            set { radius = value; }
        }
    }
}
