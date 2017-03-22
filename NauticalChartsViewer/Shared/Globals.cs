using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{
    internal static class Globals
    {
        public static double SafetyDepth = 28.0d;

        public static double SafetyContour = 10.0d;

        public static double ShallowDepth = 3.0d;

        public static double DeepDepth = 10.0d;

        public static NauticalChartsDisplayCategory DisplayMode;

        public static NauticalChartsSymbolTextDisplayMode SymbolTextDisplayMode;

        public static NauticalChartsDefaultColorSchema CurrentColorSchema;

        public static NauticalChartsSymbolDisplayMode CurrentSymbolDisplayMode;

        public static NauticalChartsBoundaryDisplayMode CurrentBoundaryDisplayMode;

        public static NauticalChartsDrawingMode CurrentDrawingMode;

        public static NauticalChartsDepthUnit CurrentDepthUnit;

        public static bool IsDepthContourTextVisible = true;

        public static bool IsFullLightLineVisible = true;

        public static bool IsMetaObjectsVisible = false;

        public static bool IsLightDescriptionVisible = true;

        public static bool IsSoundingTextVisible = true;


        static Globals()
        {
            DisplayMode = NauticalChartsDisplayCategory.All;
            SymbolTextDisplayMode = NauticalChartsSymbolTextDisplayMode.None;
        }
    }
}