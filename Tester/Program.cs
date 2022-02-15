﻿using EasyChartLib;
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
                    DecimalDigits = 1,
                    ShowAxis = true,
                    ShowLegend = true,
                    AxisMode = EAxisMode.All,
                    RanksAlpha = 128,
                    FontSize = 10, //SystemFonts.DefaultFont.Size,
                    RankColors = new List<string> { "AACCFF", "00FF44", "FFFF00", "FF0000" },
                },
                Categories = new List<SingleCategoryData>
                {
                    new SingleCategoryData
                    {
                        Name = "Dec-21",
                        Measured = 23,
                        Target = 20,
                        Ranks = new List<float?> { null, 5, 12, 17, null },
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
