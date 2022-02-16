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

        public async Task<ChartSettings> Load(string templateName)
        {
            try
            {
                var templateUri = new Uri(Settings.Default.TemplatesRootUrl + "/" + templateName + ".json");

                var wb = new WebClient();
                var templateJson = await wb.DownloadStringTaskAsync(templateUri);
                var chartSettings = JsonConvert.DeserializeObject<ChartSettings>(templateJson);
                return chartSettings;
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Template could not be found");
            }
        }


    }
}