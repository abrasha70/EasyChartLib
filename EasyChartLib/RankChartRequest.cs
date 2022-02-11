using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace EasyChartLib
{
    public class RankChartRequest
    {
        public ChartSettings Settings { get; set; }

        public List<Category> Periods { get; set; }




        public class ChartSettings
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public bool ShowAxis { get; set; }
            public bool ShowLegend { get; set; }
            public EVisibility ShowTarget { get; set; }
            public int DecimalDigits { get; set; }
            public int RanksAlpha { get; set; }
            public Dictionary<string, RankDef> RankDefs { get; set; }
            public Font Font { get; set; }
            public EAxisMode AxisMode { get; set; }

        }


        public class RankDef
        {
            public string Name { get; set; }
            public string ColorHex { get; set; }
        }

        public class Category
        {
            public string Name { get; set; }
            public float? Measured { get; set; }
            public float? Target { get; set; }
            public List<Rank> Ranks { get; set; }

        }

        public class Rank
        {
            public string Key { get; set; }
            public float? MinValue { get; set; }
            public float? MaxValue { get; set; }
        }


    }
}