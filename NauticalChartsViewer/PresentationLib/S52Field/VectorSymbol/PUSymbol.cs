using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class PUSymbol : S52BaseSymbol
    {
        private SPSymbol colorSymbol;
        private SWSymbol widthSymbol;
        private Collection<S52BaseSymbol> drawingSymbols;

        public PUSymbol()
            : this(string.Empty)
        {

        }

        public PUSymbol(string value)
            : base(value)
        {
            colorSymbol = new SPSymbol();
            widthSymbol = new SWSymbol();
            drawingSymbols = new Collection<S52BaseSymbol>();
        }

        public SPSymbol ColorSymbol
        {
            get { return colorSymbol; }
            set { colorSymbol = value; }
        }

        public SWSymbol WidthSymbol
        {
            get { return widthSymbol; }
            set { widthSymbol = value; }
        }

        public Collection<S52BaseSymbol> DrawingSymbols
        {
            get { return drawingSymbols; }
            set { drawingSymbols = value; }
        }

        public Vertex GetVertex()
        {
            string[] bodyPart = GetBody().Split(',');
            int x = Convert.ToInt32(bodyPart[0]);
            int y = Convert.ToInt32(bodyPart[1]);
            return new Vertex(x, y);
        }

        public Collection<DAIShape> GetShapes(Dictionary<string, RGBColor> colorRef)
        {
            Collection<DAIShape> shapes = new Collection<DAIShape>();

            RGBColor color = colorSymbol.GetColor(colorRef);
            int width = widthSymbol.GetWidth();

            Vertex startVertex = GetVertex();
            Collection<Vertex> lineBuffer = new Collection<Vertex>();
            lineBuffer.Insert(0, new Vertex(startVertex.X, startVertex.Y));

            foreach (S52BaseSymbol drawingSymbol in drawingSymbols)
            {
                if (drawingSymbol is PDSymbol)
                {
                    // line or point shape
                    Collection<Vertex> vertexes = (drawingSymbol as PDSymbol).GetVertexes();

                    if (vertexes.Count == 0)
                    {
                        // it's a point shape
                        PointShape shape = new PointShape(startVertex.X, startVertex.Y, width, color);
                        shapes.Add(shape);

                        // the next start vertex is the same as the current
                        // clean line buffer, and start next buffer
                        lineBuffer.Clear();
                        lineBuffer.Add(new Vertex(startVertex.X, startVertex.Y));
                    }
                    else
                    {
                        // it's a line shape
                        foreach (Vertex vertex in vertexes)
                        {
                            lineBuffer.Add(vertex);
                        }

                        // the next start vertex is the end of the line vertex
                        startVertex = vertexes[vertexes.Count - 1];
                    }
                }
                else if (drawingSymbol is CISymbol)
                {
                    // circle shape
                    // the next start vertex is the same as the current
                    int radius = (drawingSymbol as CISymbol).GetRadius();
                    CircleShape shape = new CircleShape(new Vertex(startVertex.X, startVertex.Y), radius, width, color);

                    // clean line buffer, then start next buffer
                    if (lineBuffer.Count > 1)
                    {
                        // add previous line shape
                        LineShape lineShape = new LineShape(new Collection<Vertex>(lineBuffer), width, color);
                        shapes.Add(lineShape);

                        lineBuffer.Clear();
                        lineBuffer.Add(new Vertex(startVertex.X, startVertex.Y));
                    }

                    // add circle shape
                    shapes.Add(shape);
                }
                else if (drawingSymbol is AASymbol)
                {
                    // ara angle shape
                    Vertex centerVertex = (drawingSymbol as AASymbol).GetCenterVertex();
                    int angle = (drawingSymbol as AASymbol).GetAngle();
                    ArcAngleShape shape = new ArcAngleShape(centerVertex, new Vertex(startVertex.X, startVertex.Y), angle, width, color);

                    // the next start vertex is the end of the ara angle vertex
                    startVertex = shape.GetEndofVertex();

                    // clean line buffer, then start next buffer
                    if (lineBuffer.Count > 1)
                    {
                        // add previous line shape
                        LineShape lineShape = new LineShape(new Collection<Vertex>(lineBuffer), width, color);
                        shapes.Add(lineShape);

                        lineBuffer.Clear();
                        lineBuffer.Add(new Vertex(startVertex.X, startVertex.Y));
                    }

                    // add angle shape
                    shapes.Add(shape);
                }
            }

            // clean line buffer, then start next buffer
            if (lineBuffer.Count > 1)
            {
                // add last line shape
                LineShape lineShape = new LineShape(new Collection<Vertex>(lineBuffer), width, color);
                shapes.Add(lineShape);
            }

            return shapes;
        }
    }
}
