using EasyChartLib.Model;
using System;
using System.Collections.Generic;

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

}
