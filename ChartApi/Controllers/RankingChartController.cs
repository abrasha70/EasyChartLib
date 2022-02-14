using ChartApi.Utils;
using EasyChartLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChartApi.Controllers
{
    public class RankingChartController : ApiController
    {
        private const string TEMPALTE_PARAMETER = "template";
        private const string MEASURED_PARAMETER = "measured";
        private const string TARGET_PARAMETER = "target";
        private const string RANKS_PARAMETER = "ranks";
        private readonly List<string> _reservedParameters = new List<string> { TEMPALTE_PARAMETER, MEASURED_PARAMETER, TARGET_PARAMETER, RANKS_PARAMETER };


        [HttpGet, HttpPost, HttpOptions, EnableCors(origins: "*", headers: "*", methods: "*")]
        public HttpResponseMessage SingleCategory(string template = "default")
        {
            var templateManager = new ChartTemplateManager();
            var chartSettings = templateManager.Load(template);

            //overwrite settings:
            var settingsParameters = GetRequestParameters(_reservedParameters);
            var jSettings = JToken.FromObject(chartSettings);
            foreach (var parameter in settingsParameters)
            {
                if (jSettings[parameter.Key] != null) jSettings[parameter.Key] = parameter.Value;
            }
            chartSettings = jSettings.ToObject<ChartSettings>();

            var requestParameters = GetRequestParameters();
            var chartData = new SingleCategoryData()
            {
                Measured = GetFloatFromParams(requestParameters, MEASURED_PARAMETER),
                Target = GetFloatFromParams(requestParameters, TARGET_PARAMETER),
                Ranks = GetObjectFromParams<List<float?>>(requestParameters, RANKS_PARAMETER),
            };

            var chart = new EasyChart();
            var image = chart.GenerateSingleRankChart(chartSettings, chartData);

            return GetImageResponse(image);
        }




        private Dictionary<string, string> GetRequestParameters(List<string> excludeParameters = null)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(item => item.Key, item => item.Value);
            foreach (var sysParam in excludeParameters ?? new List<string>())
            {
                if (queryParams.ContainsKey(sysParam)) queryParams.Remove(sysParam);
            }
            return queryParams;
        }

        private float? GetFloatFromParams(Dictionary<string, string> pairs, string key, float? valueForMissing = null)
        {
            if (!pairs.ContainsKey(key)) return valueForMissing;
            var value = pairs[key];
            if (string.IsNullOrEmpty(value)) return null;
            return float.Parse(value);
        }

        private T GetObjectFromParams<T>(Dictionary<string, string> pairs, string key, T valueForMissing = default(T))
        {
            if (!pairs.ContainsKey(key)) return valueForMissing;
            var value = pairs[key];
            if (string.IsNullOrEmpty(value)) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }


        public HttpResponseMessage GetImageResponse(Image image)
        {
            //var bmp = new System.Drawing.Bitmap(40, 30);
            var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "image.png";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");   //application/pdf

            return response;
        }



    }
}
