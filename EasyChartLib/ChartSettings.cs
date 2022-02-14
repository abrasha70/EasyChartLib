﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyChartLib
{

    public class ChartSettings
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        
        [JsonProperty("width")]
        public int Width { get; set; }
        
        public bool ShowAxis { get; set; }
        public bool ShowLegend { get; set; }
        public EVisibility ShowTarget { get; set; }
        public int DecimalDigits { get; set; }
        public int RanksAlpha { get; set; }
        public List<string> RankColors { get; set; }
        public List<string> RankNames { get; set; }
        public float FontSize { get; set; }
        public EAxisMode AxisMode { get; set; }
    }
}