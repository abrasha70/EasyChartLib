using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyChartLib.Model
{
    internal class LmsFileModel
    {
        public List<LmsModel> Lms { get; set; }
    }

    internal class LmsModel
    {
        public decimal Lookup { get; set; }

        public decimal L { get; set; }

        public decimal M { get; set; }

        public decimal S { get; set; }

    }

}
