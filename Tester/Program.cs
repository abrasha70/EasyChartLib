using EasyChartLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = GetSampleRequest();
            var chart = new EasyChart();

            //var image = chart.GenerateMultiRankChart(request.Settings, request.Categories);
            var image = chart.GenerateSingleRankChart(request.Settings, request.Categories[0]);
            image.Save("sample.png", ImageFormat.Png);
            System.Diagnostics.Process.Start("sample.png");
        }


        private static RankChartRequest GetSampleRequest()
        {
            var request = new RankChartRequest()
            {
                Settings = new ChartSettings
                {
                    Height = 300,
                    Width = 500,
                    ShowAxis = true,
                    ShowLegend = true,
                    AxisMode = EAxisMode.All,
                    RanksAlpha = 255,
                    FontSize = 8.5f, //SystemFonts.DefaultFont.Size,
                    RankColors = new List<string> { "b4c6e7", "c6e0b4", "ffe699", "f8cbad" },
                },
                Categories = new List<SingleCategoryData>
                {
                    new SingleCategoryData
                    {
                        Name = "Dec-21",
                        Measured = 5f,
                        Target = 7f,
                        Ranks = new List<float?> { null, 2, 4, 6, null },
                    },
                    new SingleCategoryData
                    {
                        Name = "Jan-22",
                        Measured = 20.5f,
                        Target = 18,
                        Ranks = new List<float?> { null, 5, 12, 16, null },
                    },
                    new SingleCategoryData
                    {
                        Name = "Feb-22",
                        Measured = 16.5f,
                        Target = 14,
                        Ranks = new List<float?> { null, 5, 12, 16, null },
                    },
                },
            };

            return request;
        }

    }
}
