using EasyChartLib.Model;

namespace EasyChartLib
{
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


}
