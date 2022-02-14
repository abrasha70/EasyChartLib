using EasyChartLib;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ChartApi.Utils
{
    internal class ChartTemplateManager
    {
        public ChartTemplateManager()
        {
        }

        public ChartSettings Load(string templateName)
        {
            if (templateName == "default")
            {
                var settings = new ChartSettings()
                {
                    Height = 300,
                    Width = 500,
                    DecimalDigits = 1,
                    ShowAxis = true,
                    ShowLegend = true,
                    AxisMode = EAxisMode.All,
                    RanksAlpha = 128,
                    FontSize = SystemFonts.DefaultFont.Size,
                    RankColors = new List<string> { "AACCFF", "00FF44", "FFFF00", "FF0000" },
                };
                return settings;
            }

            throw new KeyNotFoundException("Template could not be found");
        }


    }
}