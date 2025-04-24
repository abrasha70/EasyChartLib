using EasyChartLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyChartLib
{
    internal class PercentilesStats
    {

        private Dictionary<EPercentile, Dictionary<decimal, LmsStat>> _stats;

        public PercentilesStats(IEnumerable<LmsModel> lmsArray)
        {
            _stats = new Dictionary<EPercentile, Dictionary<decimal, LmsStat>>();

            foreach (EPercentile percentile in Enum.GetValues(typeof(EPercentile)))
            {
                var lmsStats = lmsArray.ToDictionary(lms => lms.Lookup, lms => new LmsStat(lms, percentile));

                _stats[percentile] = lmsStats;
            }
        }

        public double GetPercentileValue(EPercentile percentile, decimal lookup)
        {
            return GetPercentileStat(percentile,lookup).PercentileValue;
        }

        public LmsStat GetPercentileStat(EPercentile percentile, decimal lookup)
        {
            var stats = GetPercentileStats(percentile);
            return _stats[percentile][lookup];
        }

        public IEnumerable<LmsStat> GetPercentileStats(EPercentile percentile)
        {
            return _stats[percentile].Values;
        }
    }


}
