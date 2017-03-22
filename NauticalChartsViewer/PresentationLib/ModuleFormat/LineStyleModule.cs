using System;

namespace ThinkGeo.MapSuite
{
    public class LineStyleModule : ChartStyleModule
    {
        public LineStyleModule()
            : base()
        {
            base.CommandType = 'V';
        }

        public override char CommandType
        {
            get
            {
                return base.CommandType;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
