using System.Drawing;

namespace EasyChartLib
{
    internal class PercentileDrawSetting
    {
        public EPercentile Percentile { get; internal set; }
        public Brush GraphBelowBrush { get; internal set; }
        public Pen GraphLinePen { get; internal set; }
    }
}