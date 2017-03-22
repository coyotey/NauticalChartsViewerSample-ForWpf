
namespace ThinkGeo.MapSuite
{
    internal class PFSymbol : S52BaseSymbol
    {
        public PFSymbol()
            : this(string.Empty)
        {
            
        }

        public PFSymbol(string value)
            : base(value)
        {
            
        }

        public AreaShapeFillPattern GetFillPattern()
        {
            if (Value.Equals("EP"))
            {
                return AreaShapeFillPattern.OutLine;
            }
            else if (Value.Equals("FP"))
            {
                return AreaShapeFillPattern.Fill;
            }
            else
            {
                return AreaShapeFillPattern.OutLine;
            }
        }
    }
}
