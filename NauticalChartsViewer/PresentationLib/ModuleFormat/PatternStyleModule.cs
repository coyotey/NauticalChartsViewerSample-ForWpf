
namespace ThinkGeo.MapSuite
{
    public class PatternStyleModule : ChartStyleModule
    {
        // PATP
        private string fillType;

        // PAMA
        private int maxDistance;

        // PAMI
        private int minDistance;

        // PASP
        private string symbolSpacingType;

        public PatternStyleModule()
            : base()
        {
            fillType = string.Empty;
            symbolSpacingType = string.Empty;
            minDistance = 0;
            maxDistance = 0;
        }

        public string FillType
        {
            get
            {
                return fillType;
            }
            set
            {
                fillType = value;
            }
        }

        public int MaxDistance
        {
            get
            {
                return maxDistance;
            }
            set
            {
                maxDistance = value;
            }
        }

        public int MinDistance
        {
            get
            {
                return minDistance;
            }
            set
            {
                minDistance = value;
            }
        }

        public string SymbolSpacingType
        {
            get
            {
                return symbolSpacingType;
            }
            set
            {
                symbolSpacingType = value;
            }
        }
    }
}