using System;
using System.IO;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;

namespace NauticalChartsViewer
{
    public class ChartItem
    {
        private string fileName;
        private RectangleShape boundingBox;
        private static readonly object helperObject = new object();

        public ChartItem()
            : this(string.Empty, null)
        {
        }


        public ChartItem(string fileName)
            : this(fileName, null)
        {
        }

        public ChartItem(string fileName, RectangleShape rectangleShape)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                this.fileName = fileName;
            }
            boundingBox = rectangleShape;
        }

        public string Name 
        {
            get
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    return Path.GetFileNameWithoutExtension(fileName);
                }
                return string.Empty;
            }

        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string IndexFileName
        {
            get 
            {
                string fileNameWithoutEx = Path.GetFileNameWithoutExtension(fileName);
                string indexFileName = Path.Combine(Path.GetDirectoryName(fileName), string.Format("{0}.idx", fileNameWithoutEx));
                return indexFileName;
            }
        }

        public RectangleShape BoundingBox
        {
            get
            {
                if (boundingBox == null)
                {
                    lock (helperObject)
                    {
                        if (boundingBox == null)
                        {
                            NauticalChartsFeatureLayer layer = new NauticalChartsFeatureLayer(FileName);
                            layer.Open();
                            boundingBox = layer.GetBoundingBox();
                            layer.Close();
                        }
                    }
                }

                return boundingBox;
            }
        }

    }
}
