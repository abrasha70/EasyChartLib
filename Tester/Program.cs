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

            var image = chart.GenerateMultiRankChart(request.Settings, request.Periods);
            image.Save("sample.png", ImageFormat.Png);
            System.Diagnostics.Process.Start("sample.png");
        }


        private static RankChartRequest GetSampleRequest()
        {
            var request = new RankChartRequest()
            {
                Settings = new RankChartRequest.ChartSettings
                {
                    Height = 300,
                    Width = 500,
                    DecimalDigits = 1,
                    ShowAxis = true,
                    ShowLegend = true,
                    ShowTarget = EVisibility.Visible,
                    AxisMode = EAxisMode.FocusedAndAround,
                    RanksAlpha = 128,
                    Font = SystemFonts.DefaultFont,

                    RankDefs = new Dictionary<string, RankChartRequest.RankDef>
                    {
                        {
                            "low",
                            new RankChartRequest.RankDef
                            {
                                Name = "Low",
                                ColorHex = "AACCFF",
                            }
                        },
                        {
                            "norm",
                            new RankChartRequest.RankDef
                            {
                                Name = "Normal",
                                ColorHex = "00FF44",
                            }
                        },
                        {
                            "high",
                            new RankChartRequest.RankDef
                            {
                                Name = "High",
                                ColorHex = "FFFF00",
                            }
                        },
                        {
                            "high2",
                            new RankChartRequest.RankDef
                                {
                                Name = "Very High",
                                ColorHex = "FF0000",
                            }
                        },
                    },
                },
                Periods = new List<RankChartRequest.Category>
                {
                    new RankChartRequest.Category
                    {
                        Name = "Dec-21",
                        Measured = 23,
                        Target = 20,
                        Ranks = new List<RankChartRequest.Rank>
                        {
                            new RankChartRequest.Rank
                            {
                                Key = "high2",
                                MinValue = 17,
                                MaxValue = null
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "high",
                                MinValue = 12,
                                MaxValue = 17
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "norm",
                                MinValue = 5,
                                MaxValue = 12
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "low",
                                MinValue = null,
                                MaxValue = 5
                            },
                        }
                    },
                    new RankChartRequest.Category
                    {
                        Name = "Jan-22",
                        Measured = 20.5f,
                        Target = 18,
                        Ranks = new List<RankChartRequest.Rank>
                        {
                            new RankChartRequest.Rank
                            {
                                Key = "high2",
                                MinValue = 16,
                                MaxValue = null
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "high",
                                MinValue = 12,
                                MaxValue = 16
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "norm",
                                MinValue = 5,
                                MaxValue = 12
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "low",
                                MinValue = null,
                                MaxValue = 5
                            },
                        }
                    },
                    new RankChartRequest.Category
                    {
                        Name = "Feb-22",
                        Measured = 16.5f,
                        Target = 15,
                        Ranks = new List<RankChartRequest.Rank>
                        {
                            new RankChartRequest.Rank
                            {
                                Key = "high2",
                                MinValue = 16,
                                MaxValue = null
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "high",
                                MinValue = 12,
                                MaxValue = 16
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "norm",
                                MinValue = 5,
                                MaxValue = 12
                            },
                            new RankChartRequest.Rank
                            {
                                Key = "low",
                                MinValue = null,
                                MaxValue = 5
                            },
                        }
                    },
                },
            };

            return request;
        }

    }
}
