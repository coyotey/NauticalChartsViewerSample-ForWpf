using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ThinkGeo.MapSuite
{
    internal class S52ObjectsPresenter : IDisposable
    {
        private Graphics graphics;
        private Bitmap bitmap;

        public S52ObjectsPresenter(int imageWidth, int imageHeight)
        {
            this.bitmap = new Bitmap(imageWidth, imageHeight);
            this.graphics = Graphics.FromImage(bitmap);
        }

    
        public void Clear(RGBColor backgroundColor)
        {
            Color color = Color.FromArgb(backgroundColor.R, backgroundColor.G, backgroundColor.B);
            graphics.Clear(color);
        }

        public void Draw(S52Object drawingObject, RGBColor backgroundColor)
        {
            MappingConverter converter = new MappingConverter(drawingObject.UpperLeftVertex,
            drawingObject.PivotVertex,
            drawingObject.Width + drawingObject.UpperLeftVertex.X,
            drawingObject.Height + drawingObject.UpperLeftVertex.Y,
            graphics.VisibleClipBounds.Width,
            graphics.VisibleClipBounds.Height);

            DrawShapes(drawingObject.Shapes, backgroundColor, converter);
            DrawPivot(drawingObject.PivotVertex, converter);
            
        }

        public Bitmap GetBitmap() 
        {
            return bitmap;
        }

        private void DrawPivot(Vertex pivotVertex, MappingConverter converter)
        {
            Color fillColor = Color.Red;
            Color outLineColor = Color.DarkBlue;
            SolidBrush brush = new SolidBrush(fillColor);
            Pen pen = new Pen(outLineColor, 2);

            PointF point = converter.GetMappingPoint(pivotVertex);
            Vertex rightMostVertex = new Vertex(pivotVertex.X + 10, pivotVertex.Y);
            //float radius = converter.GetMappingDistance(pivotVertex, rightMostVertex);
            float radius = 5f;

            graphics.FillEllipse(brush, point.X - radius, point.Y - radius, radius * 2, radius * 2);
            graphics.DrawEllipse(pen, point.X - radius, point.Y - radius, radius * 2, radius * 2);
        }

        private void DrawShapes(Collection<DAIShape> shapes, RGBColor backgroundColor, MappingConverter converter)
        {
            foreach (DAIShape shape in shapes)
            {
                if (shape.ShapeType == DAIShapeType.Point)
                {
                    DrawPoint(shape as PointShape, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Line)
                {
                    DrawLine(shape as LineShape, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Circle)
                {
                    DrawCircle(shape as CircleShape, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Area)
                {
                    DrawArea(shape as AreaShape, backgroundColor, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Angle)
                {
                    // can not be supported
                }
            }
        }

        private void DrawPoint(PointShape shape, MappingConverter converter)
        {
            Color color = Color.FromArgb(shape.Color.R, shape.Color.G, shape.Color.B);
            SolidBrush brush = new SolidBrush(color);

            Vertex center = new Vertex(shape.X, shape.Y);
            PointF point = converter.GetMappingPoint(center);
            float radius = converter.GetPenWidth(shape.Width) / 2f;

            graphics.FillEllipse(brush, point.X - radius, point.Y - radius, radius * 2, radius * 2);
        }

        private void DrawLine(LineShape shape, MappingConverter converter)
        {
            Color color = Color.FromArgb(shape.Color.R, shape.Color.G, shape.Color.B);
            Pen pen = new Pen(color, converter.GetPenWidth(shape.Width));

            PointF[] points = new PointF[shape.Vertexes.Count];
            for (int i = 0; i < shape.Vertexes.Count; i++)
            {
                points[i] = converter.GetMappingPoint(shape.Vertexes[i]);
            }

            if (shape.Vertexes[0].GeometryEqual(shape.Vertexes[shape.Vertexes.Count - 1]))
            {
                graphics.DrawPolygon(pen, points);
            }
            else
            {
                graphics.DrawLines(pen, points);
                DrawPoint(new PointShape(shape.Vertexes[0], shape.Width, shape.Color), converter);
                DrawPoint(new PointShape(shape.Vertexes[shape.Vertexes.Count - 1], shape.Width, shape.Color), converter);
            }
        }

        private void DrawCircle(CircleShape shape, MappingConverter converter)
        {
            Color color = Color.FromArgb(shape.Color.R, shape.Color.G, shape.Color.B);
            Pen pen = new Pen(color, converter.GetPenWidth(shape.Width));
            PointF center = converter.GetMappingPoint(shape.Center);

            Vertex rightMostVertex = new Vertex(shape.Center.X + shape.Raduis, shape.Center.Y);
            float radius = converter.GetMappingDistance(shape.Center, rightMostVertex);

            graphics.DrawEllipse(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        }

        private void DrawArea(AreaShape shape, RGBColor backgroundColor, MappingConverter converter)
        {
            if (shape.FillPattern == AreaShapeFillPattern.Fill)
            {
                Color fillOuterColor = Color.FromArgb(shape.Color.R, shape.Color.G, shape.Color.B);
                Color fillInnerColor = Color.FromArgb(backgroundColor.R, backgroundColor.G, backgroundColor.B);

                foreach (RingShape ring in shape.OuterRings)
                {
                    FillRing(ring, fillOuterColor, converter);
                }

                foreach (RingShape ring in shape.InnerRings)
                {
                    FillRing(ring, fillInnerColor, converter);
                }
            }
            else
            {
                foreach (RingShape ring in shape.OuterRings)
                {
                    DrawRing(ring, converter);
                }

                foreach (RingShape ring in shape.InnerRings)
                {
                    DrawRing(ring, converter);
                }
            }
        }

        private void DrawRing(RingShape ring, MappingConverter converter)
        {
            foreach (DAIShape shape in ring.Shapes)
            {
                if (shape.ShapeType == DAIShapeType.Line)
                {
                    DrawLine(shape as LineShape, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Circle)
                {
                    DrawCircle(shape as CircleShape, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Angle)
                {
                    // TODO : can not be supported
                }
            }
        }

        private void FillRing(RingShape ring, Color fillColor, MappingConverter converter)
        {
            Collection<LineShape> lineShapes = new Collection<LineShape>();

            foreach (DAIShape shape in ring.Shapes)
            {
                if (shape.ShapeType == DAIShapeType.Line)
                {
                    FillLineShape(shape as LineShape, fillColor, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Circle)
                {
                    FillCircleShape(shape as CircleShape, fillColor, converter);
                }
                else if (shape.ShapeType == DAIShapeType.Angle)
                {
                    // TODO : can not be supported
                }
            }
        }

        private void FillLineShape(LineShape shape, Color fillColor, MappingConverter converter)
        {
            SolidBrush brush = new SolidBrush(fillColor);
            PointF[] points = new PointF[shape.Vertexes.Count];

            for (int i = 0; i < shape.Vertexes.Count; i++)
            {
                points[i] = converter.GetMappingPoint(shape.Vertexes[i]);
            }

            graphics.FillPolygon(brush, points);
        }

        private void FillCircleShape(CircleShape shape, Color fillColor, MappingConverter converter)
        {
            SolidBrush brush = new SolidBrush(fillColor);
            PointF center = converter.GetMappingPoint(shape.Center);

            Vertex rightMostVertex = new Vertex(shape.Center.X + shape.Raduis, shape.Center.Y);
            float radius = converter.GetMappingDistance(shape.Center, rightMostVertex);

            graphics.FillEllipse(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        }

        public void Close()
        {
            graphics.Dispose();
            bitmap.Dispose();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
