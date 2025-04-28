using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyChartLib
{
    public class LmsChartParameters
    {
        public LmsChartSettings Settings { get; set; }

        public List<LmsMeasurement> Measurements { get; set; }
    }

    public class LmsMeasurement
    {
        public float Lookup { get; set; }

        public float? MeasuredValue { get; set; }
    }


}