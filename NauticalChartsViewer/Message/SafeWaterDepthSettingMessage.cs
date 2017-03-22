using GalaSoft.MvvmLight.Messaging;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{
    public class SafeWaterDepthSettingMessage : MessageBase
    {
        private double safeWaterDepth;
        private double shallowWaterDepth;
        private double deepWaterDepth;
        private double safetyContourDepth;
        private NauticalChartsDepthUnit depthUnit;


        public SafeWaterDepthSettingMessage() { }

        public SafeWaterDepthSettingMessage(double safetyContourDepth, double safeWaterDepth, double shallowWaterDepth, double deepWaterDepth, NauticalChartsDepthUnit depthUnit)
        {
            this.safetyContourDepth = safetyContourDepth;
            this.safeWaterDepth = safeWaterDepth;
            this.shallowWaterDepth = shallowWaterDepth;
            this.deepWaterDepth = deepWaterDepth;
            this.depthUnit = depthUnit;
        }

        public double SafetyWaterDepth
        {
            get { return safeWaterDepth; }
            set { safeWaterDepth = value; }
        }

        public double SafetyContourDepth
        {
            get { return safetyContourDepth; }
            set { safetyContourDepth = value; }
        }


        public double ShallowWaterDepth
        {
            get { return shallowWaterDepth; }
            set { shallowWaterDepth = value; }
        }

        public double DeepWaterDepth
        {
            get { return deepWaterDepth; }
            set { deepWaterDepth = value; }
        }

        public NauticalChartsDepthUnit DepthUnit
        {
            get { return depthUnit; }
            set { depthUnit = value; }
        }
    }
}
