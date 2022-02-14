using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyChartLib
{
    public partial class RankChartRequest
    {
        public ChartSettings Settings { get; set; }

        public List<Category> Periods { get; set; }



        public class Category
        {
            public string Name { get; set; }
            public float? Measured { get; set; }
            public float? Target { get; set; }
            public List<float?> Ranks { get; set; }

            internal List<RankRange> GetRanksAsRanges()
            {
                var result = new List<RankRange>();
                for (int index = 1; index < Ranks.Count; index++)
                {
                    var newDef = new RankRange
                    {
                        Index = index - 1,
                        FromValue = Ranks[index - 1],
                        ToValue = Ranks[index],
                    };
                    result.Add(newDef);
                }
                return result;
            }
        }

    }
}