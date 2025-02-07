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

        [JsonProperty("font_size")]
        public float FontSize { get; set; }

        [JsonProperty("axis_mode")]
        public EZoomMode ZoomMode { get; set; }

    }

    public class RanksChartSettings : ChartSettings
    {
        [JsonProperty("show_Legend")]
        public bool ShowLegend { get; set; }

        [JsonProperty("ranks_alpha")]
        public int RanksAlpha { get; set; }

        [JsonProperty("rank_colors")]
        public List<string> RankColors { get; set; }

        [JsonProperty("rank_names")]
        public List<string> RankNames { get; set; }
    }

    public class LmsChartSettings : ChartSettings
    {
        [JsonProperty("lms_file")]
        public string LmsFile { get; set; }
    }
}