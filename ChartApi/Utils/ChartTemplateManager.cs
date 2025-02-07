using ChartApi.Properties;
using EasyChartLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

namespace ChartApi.Utils
{
    internal class ChartTemplateManager
    {
        public ChartTemplateManager()
        {
        }

        public async Task<RanksChartSettings> Load(string templateName)
        {
            if (templateName == null)
            {
                var defaultTemplate = new RanksChartSettings
                {
                    Height = 300,
                    Width = 500,
                    ShowAxis = true,
                    ShowLegend = false,
                    RanksAlpha = 128,
                    RankColors = null,
                    RankNames = null,
                    FontSize = 10,
                    ZoomMode = EZoomMode.All,
                };
                return defaultTemplate;
            }

            try
            {
                var templateUri = new Uri(Settings.Default.TemplatesRootUrl + "/" + templateName + ".json");

                var wb = new WebClient();
                var templateJson = await wb.DownloadStringTaskAsync(templateUri);
                var chartSettings = JsonConvert.DeserializeObject<RanksChartSettings>(templateJson);
                return chartSettings;
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Template could not be found");
            }
        }


    }
}