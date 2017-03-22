namespace ThinkGeo.MapSuite
{
    internal abstract class DAIShape
    {
        private DAIShapeType shapeType;
        private int width;
        private RGBColor color;

        public DAIShape(DAIShapeType shapeType)
            : this(shapeType, 1, new RGBColor())
        {
        }

        public DAIShape(DAIShapeType shapeType, int width, RGBColor color)
        {
            this.shapeType = shapeType;
            this.width = width;
            this.color = color;
        }

        public DAIShapeType ShapeType
        {
            get { return shapeType; }
            set { shapeType = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public RGBColor Color
        {
            get { return color; }
            set { color = value; }
        }

    }
}
