namespace ThinkGeo.MapSuite
{
    internal class ArcAngleShape : DAIShape
    {
        private Vertex start;
        private Vertex center;
        private int angle;

        public ArcAngleShape()
            : this(new Vertex(), new Vertex(), 0, 1, new RGBColor())
        {
            
        }

        public ArcAngleShape(Vertex center, Vertex start, int angle)
            : this(center, start, angle, 1, new RGBColor())
        {
            
        }

        public ArcAngleShape(Vertex center, Vertex start, int angle, int width, RGBColor color)
            : base(DAIShapeType.Angle, width, color)
        {
            this.center = center;
            this.start = start;
            this.angle = angle;
        }

        public Vertex Center
        {
            get { return center; }
            set { center = value; }
        }

        public Vertex Start
        {
            get { return start; }
            set { start = value; }
        }

        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Vertex GetEndofVertex()
        {
            // TODO : this method have not enough information to achieve
            return null;
        }
    }
}
