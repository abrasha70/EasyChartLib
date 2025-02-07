using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyChartLib
{
    public partial class RankChartRequest
    {
        public RanksChartSettings Settings { get; set; }

        public List<SingleCategoryData> Categories { get; set; }

    }

    public class SingleCategoryData
    {
        public string Name { get; set; }
        public float? Measured { get; set; }
        public float? Target { get; set; }
        public List<float?> RankLevels { get; set; }


        internal List<RankRange> GetRanksAsRanges()
        {
            if (RankLevels == null) return new List<RankRange>();

            var result = new List<RankRange>();
            for (int index = 1; index < RankLevels.Count; index++)
            {
                var newDef = new RankRange
                {
                    Index = index - 1,
                    FromValue = RankLevels[index - 1],
                    ToValue = RankLevels[index],
                };
                result.Add(newDef);
            }
            return result;
        }
    }

    public class LmsMeasurement
    {
        public float LookupValue { get; set; }

        public float ComparedValue { get; set; }
    }


}