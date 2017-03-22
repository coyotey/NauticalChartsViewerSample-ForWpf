using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class RingShape : DAIShape
    {
        private Collection<DAIShape> ring;

        public RingShape()
            : this(new Collection<DAIShape>(), 0, new RGBColor())
        {
        }

        public RingShape(Collection<DAIShape> ring)
            : this(ring, 0, new RGBColor())
        {
        }


        public RingShape(Collection<DAIShape> ring, int width, RGBColor color)
            : base(DAIShapeType.Ring, width, color)
        {
            this.ring = ring;
        }

        public Collection<DAIShape> Shapes
        {
            get { return ring; }
        }
    }
}
