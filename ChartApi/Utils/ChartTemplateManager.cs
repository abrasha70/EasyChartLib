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
                    ShowAxis = true,
                    ShowLegend = true,
                    AxisMode = EAxisMode.All,
                    RanksAlpha = 255,
                    FontSize = 8.5f, //SystemFonts.DefaultFont.Size,
                    RankColors = new List<string> { "b4c6e7", "c6e0b4", "ffe699", "f8cbad" },
                };
                return settings;
            }

            throw new KeyNotFoundException("Template could not be found");
        }


    }
}