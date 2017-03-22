
namespace ThinkGeo.MapSuite
{
    public class Vertex
    {
        private int x;
        private int y;

        public Vertex()
            : this(0, 0)
        {

        }

        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool GeometryEqual(Vertex vertex)
        {
            if (this.x == vertex.x && this.y == vertex.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
