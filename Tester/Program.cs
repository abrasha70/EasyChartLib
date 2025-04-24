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
            var request = GetSampleLmsChartRequest();
            var chart = new EasyChart();

            var image = chart.GenerateLmsChart(request.Settings, request.Measurements);
            image.Save("sample.png", ImageFormat.Png);
            System.Diagnostics.Process.Start("sample.png");



            //var request = GetSampleRankChartRequest();
            //var chart = new EasyChart();

            ////var image = chart.GenerateMultiRankChart(request.Settings, request.Categories);
            //var image = chart.GenerateSingleRankChart(request.Settings, request.Categories[0]);
            //image.Save("sample.png", ImageFormat.Png);
            //System.Diagnostics.Process.Start("sample.png");
        }

        private static LmsChartParameters GetSampleLmsChartRequest()
        {
            var request = new LmsChartParameters
            {
                Settings = new LmsChartSettings
                {
                    SourceKey = "BmiForAgeLmsByCdc",    //"BmiForAgeLmsByWho",
                    SegmentKey = "Boys2-20",           //"Boys0-5"
                    Height = 300,
                    Width = 500,
                    ShowAxis = true,
                    ZoomMode = EZoomMode.FocusedAndNearby,
                    FontSize = 10f, //SystemFonts.DefaultFont.Size,
                },
                Measurements = new List<LmsMeasurement>
                {
                    new LmsMeasurement
                    {
                        Lookup = 10,
                        MeasuredValue = 16.8f,
                    },
                    new LmsMeasurement
                    {
                        Lookup = 8,
                        MeasuredValue = 15,
                    },
                    new LmsMeasurement
                    {
                        Lookup = 7,
                        MeasuredValue = 14.5f,
                    }
                }
            };

            return request;
        }


        private static RankChartParameters GetSampleRankChartRequest()
        {
            var request = new RankChartParameters
            {
                Settings = new RanksChartSettings
                {
                    Height = 90,
                    Width = 500,
                    ShowAxis = true,
                    ShowLegend = true,
                    ZoomMode = EZoomMode.FocusedAndNearby,
                    RanksAlpha = 255,
                    FontSize = 10f, //SystemFonts.DefaultFont.Size,
                    //RankColors = new List<string> { "b4c6e7", "c6e0b4", "ffe699", "f8cbad" },   //can be overwrite from the client
                    //RankNames = new List<string> { "low", "normal", "high", "very high" },      //can be overwrite from the client
                },
                Categories = new List<SingleCategoryData>
                {
                    new SingleCategoryData
                    {
                        Name = "Dec-21",
                        Measured = 26f,
                        Target = 25f,
                        RankLevels = new List<float?> { null,14 ,25 ,32 ,null },
                    },
                    new SingleCategoryData
                    {
                        Name = "Jan-22",
                        Measured = 20.5f,
                        Target = 18,
                        RankLevels = new List<float?> { null, 5, 12, 16, null },
                    },
                    new SingleCategoryData
                    {
                        Name = "Feb-22",
                        Measured = 16.5f,
                        Target = 14,
                        RankLevels = new List<float?> { null, 5, 12, 16, null },
                    },
                },
            };

            return request;
        }

    }
}
