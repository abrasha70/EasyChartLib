using ChartApi.Properties;
using EasyChartLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace ChartApi.Utils
{
    internal class ChartTemplateManager
    {
        public ChartTemplateManager()
        {
        }

        public ChartSettings Load(string templateName)
        {
            try
            {
                var templateUri = new Uri(Settings.Default.TemplatesRootUrl + "/" + templateName + ".json");

                var wb = new WebClient();
                var templateJson = wb.DownloadString(templateUri);
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