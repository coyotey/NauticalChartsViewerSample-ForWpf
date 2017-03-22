using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class PMSymbol : S52BaseSymbol
    {
        private STSymbol transparencySymbol;
        private PFSymbol fillPatternSymbol;
        private Collection<PUSymbol> outerRingSymbols;
        private Collection<PUSymbol> innerRingSymbols;

        public PMSymbol()
            : base(string.Empty)
        {
            this.transparencySymbol = new STSymbol();
            this.fillPatternSymbol = new PFSymbol();
            this.outerRingSymbols = new Collection<PUSymbol>();
            this.innerRingSymbols = new Collection<PUSymbol>();
        }

        public Collection<PUSymbol> OuterRingSymbols
        {
            get { return outerRingSymbols; }
            set { outerRingSymbols = value; }
        }

        public Collection<PUSymbol> InnerRingSymbols
        {
            get { return innerRingSymbols; }
            set { innerRingSymbols = value; }
        }

        public STSymbol TransparencySymbol
        {
            get { return transparencySymbol; }
            set { transparencySymbol = value; }
        }

        public PFSymbol FillPatternSymbol
        {
            get { return fillPatternSymbol; }
            set { fillPatternSymbol = value; }
        }

        public AreaShape GetAreaShape(Dictionary<string, RGBColor> colorRef)
        {
            AreaShape shape = new AreaShape();
            shape.Transparency = transparencySymbol.GetTransparency();
            shape.FillPattern = fillPatternSymbol.GetFillPattern();

            if (innerRingSymbols.Count == 0)
            {
                // it only has outer ring
                shape.OuterRings = GetRing(outerRingSymbols, colorRef);
            }
            else
            {
                // it has inner rings
                shape.OuterRings = GetRing(outerRingSymbols, colorRef);
                shape.InnerRings = GetRing(innerRingSymbols, colorRef);
            }

            shape.Color = shape.OuterRings[0].Color;
            shape.Width = shape.OuterRings[0].Width;

            return shape;
        }

        private Collection<RingShape> GetRing(Collection<PUSymbol> PUSymbols, Dictionary<string, RGBColor> colorRef)
        {
            Collection<RingShape> shapes = new Collection<RingShape>();

            for (int i = 0; i < PUSymbols.Count; i++)
            {
                Collection<DAIShape> drawingShapes = PUSymbols[i].GetShapes(colorRef);
                RingShape ring = new RingShape(drawingShapes);
                ring.Color = drawingShapes[0].Color;
                ring.Width = drawingShapes[0].Width;

                shapes.Add(ring);
            }

            return shapes;
        }
    }
}
