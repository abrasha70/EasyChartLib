using EasyChartLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChartLib
{
    internal static class LmsCalc
    {
        // Common percentile Z-scores
        private static readonly Dictionary<int, double> PercentileToZ = new Dictionary<int, double>
        {
            { 3, -1.88079 },
            { 5, -1.64485 },
            { 10, -1.28155 },
            { 15, -1.03643 },
            { 25, -0.67449 },
            { 50, 0.0 },
            { 75, 0.67449 },
            { 85, 1.03643 },
            { 90, 1.28155 },
            { 95, 1.64485 },
            { 97, 1.88079 }
        };

        public static double GetLmsValue(double L, double M, double S, int percentile)
        {
            if (!PercentileToZ.ContainsKey(percentile))
                throw new ArgumentException("Unsupported percentile");

            double Z = PercentileToZ[percentile];

            if (L == 0)
            {
                return M * Math.Exp(S * Z);
            }
            else
            {
                return M * Math.Pow(1 + L * S * Z, 1 / L);
            }
        }

        public static double GetLmsValue(LmsModel lms, int percentile)
        {
            return GetLmsValue((double)lms.L, (double)lms.M, (double)lms.S, percentile);
        }

    }


    internal enum EPercentile
    {
        Perc3 = 3,
        Perc10 = 10,
        Perc25 = 25,
        Perc50 = 50,
        Perc75 = 75,
        Perc90 = 90,
        Perc97 = 97,
    }

    internal class LmsStat
    {
        public LmsModel lmsModel { get; private set; }

        public EPercentile Percentile { get; private set; }

        public decimal Lookup => lmsModel.Lookup;

        public double PercentileValue { get; private set; }


        public LmsStat(LmsModel lms, EPercentile percentile)
        {
            lmsModel = lms;

            Percentile = percentile;

            PercentileValue = LmsCalc.GetLmsValue(lms, (int)percentile);
        }
    }




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
