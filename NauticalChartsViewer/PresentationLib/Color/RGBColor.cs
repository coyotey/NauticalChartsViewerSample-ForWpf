namespace ThinkGeo.MapSuite
{
    public class RGBColor
    {
        private int r;
        private int g;
        private int b;

        public RGBColor()
            : this(0, 0, 0)
        {
        }

        public RGBColor(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public int R
        {
            get { return r; }
            set { r = value; }
        }

        public int G
        {
            get { return g; }
            set { g = value; }
        }

        public int B
        {
            get { return b; }
            set { b = value; }
        }
    }
}