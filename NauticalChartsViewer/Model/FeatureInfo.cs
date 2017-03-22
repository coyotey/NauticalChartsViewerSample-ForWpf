using System.Collections.Generic;
using ThinkGeo.MapSuite.Shapes;

namespace NauticalChartsViewer
{
    public class FeatureInfo : ObservableObject
    {
        private Dictionary<string, string> columnValues;
        private string id;
        private string layerName;
        private WellKnownType geometry;
        private double area = double.MaxValue;
        
        public FeatureInfo(Feature feature, string layerName, double area)
        {
            this.id = feature.Id;
            this.layerName = layerName;
            this.geometry = feature.GetWellKnownType();
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (var key in feature.ColumnValues.Keys)
            {
                if (!string.IsNullOrEmpty(feature.ColumnValues[key]))
                {
                    temp.Add(key, feature.ColumnValues[key]);
                }
            }

            this.columnValues = temp;
            this.area = area;
        }

        public string Id
        {
            get { return id; }
        }

        public string LayerName
        {
            get { return layerName; }
        }

        public double Area
        {
            get { return area; }
        }

        public WellKnownType Geometry
        {
            get { return geometry; }
        }

        public Dictionary<string, string> ColumnValues
        {
            get { return columnValues; }
        }
    }
}
