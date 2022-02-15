using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyChartLib
{

    public class ChartSettings
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("show_axis")]
        public bool ShowAxis { get; set; }

        [JsonProperty("show_Legend")]
        public bool ShowLegend { get; set; }
        public int RanksAlpha { get; set; }
        public List<string> RankColors { get; set; }
        public List<string> RankNames { get; set; }
        public float FontSize { get; set; }
        public EAxisMode AxisMode { get; set; }
    }
}