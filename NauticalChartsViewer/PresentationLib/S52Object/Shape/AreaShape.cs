using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class AreaShape : DAIShape
    {
        private Collection<RingShape> outerRings;
        private Collection<RingShape> innerRings;
        private int transparency;
        private AreaShapeFillPattern fillPattern;

        public AreaShape()
            : this(new Collection<RingShape>(), new Collection<RingShape>(), 1, new RGBColor(), 0, AreaShapeFillPattern.OutLine)
        {

        }

        public AreaShape(Collection<RingShape> outerRings, Collection<RingShape> innerRings)
            : this(outerRings, innerRings, 1, new RGBColor(), 0, AreaShapeFillPattern.OutLine)
        {

        }

        public AreaShape(Collection<RingShape> outerRings, Collection<RingShape> innerRings, int width, RGBColor color, int transparency, AreaShapeFillPattern fillPattern)
            : base(DAIShapeType.Area, width, color)
        {
            this.outerRings = outerRings;
            this.innerRings = outerRings;
            this.transparency = transparency;
            this.fillPattern = fillPattern;
        }

        public Collection<RingShape> OuterRings
        {
            get { return outerRings; }
            set { outerRings = value; }
        }

        public Collection<RingShape> InnerRings
        {
            get { return innerRings; }
            set { innerRings = value; }
        }

        public int Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        public AreaShapeFillPattern FillPattern
        {
            get { return fillPattern; }
            set { fillPattern = value; }
        }

    }
}
