using System.Collections.Generic;
using System.Drawing;

namespace EasyChartLib
{

    public class ChartSettings
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public bool ShowAxis { get; set; }
        public bool ShowLegend { get; set; }
        public EVisibility ShowTarget { get; set; }
        public int DecimalDigits { get; set; }
        public int RanksAlpha { get; set; }
        //public Dictionary<string, RankDef> RankDefs { get; set; }
        public List<string> RankColors { get; set; }
        public List<string> RankNames { get; set; }
        public float FontSize { get; set; }
        public EAxisMode AxisMode { get; set; }
    }

    internal class LoadedSettings
    {
        public ChartSettings Raw { get; private set; }
        public Font Font { get; set; }


        public LoadedSettings(ChartSettings chartSettings)
        {
            Raw = chartSettings;
            Font = new Font(SystemFonts.DefaultFont.FontFamily, chartSettings.FontSize);
        }


    }


    //public class RankDef
    //{
    //    public string Name { get; set; }
    //    public string ColorHex { get; set; }
    //}


}