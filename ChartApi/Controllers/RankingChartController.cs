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
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChartApi.Controllers
{
    public class RankingChartController : ApiController
    {
        private const string TEMPALTE_PARAMETER = "template";
        private const string MEASURED_PARAMETER = "measured";
        private const string TARGET_PARAMETER = "target";
        private const string RANK_LEVELS_PARAMETER = "rank_levels";
        private readonly List<string> _reservedParameters = new List<string> { TEMPALTE_PARAMETER, MEASURED_PARAMETER, TARGET_PARAMETER, RANK_LEVELS_PARAMETER };


        [HttpGet, HttpPost, HttpOptions, EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> SingleCategory(string template = null)
        {
            var templateManager = new ChartTemplateManager();
            var chartSettings = await templateManager.Load(template);

            //overwrite settings:
            var settingsParameters = GetRequestParameters(_reservedParameters);
            var jsonPropToProp = JsonPropToProp(typeof(ChartSettings));
            foreach (var parameter in settingsParameters)
            {
                var propName = parameter.Key;
                if (!jsonPropToProp.ContainsKey(propName)) continue;
                var settingsProperty = jsonPropToProp[propName];

                if (settingsProperty.PropertyType.IsClass)
                {
                    var value = JsonConvert.DeserializeObject(parameter.Value, settingsProperty.PropertyType);
                    settingsProperty.SetValue(chartSettings, value);
                }
                else
                {
                    settingsProperty.SetValue(chartSettings, parameter.Value);
                }
            }

            var requestParameters = GetRequestParameters();
            var chartData = new SingleCategoryData()
            {
                Measured = GetFloatFromParams(requestParameters, MEASURED_PARAMETER),
                Target = GetFloatFromParams(requestParameters, TARGET_PARAMETER),
                RankLevels = GetObjectFromParams<List<float?>>(requestParameters, RANK_LEVELS_PARAMETER),
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



        private Dictionary<string, PropertyInfo> JsonPropToProp(Type type)
        {
            var settingsProperties = type.GetProperties();
            var jsonPropToProp = settingsProperties.ToDictionary(prop => prop.CustomAttributes.Where(att => att.AttributeType == typeof(JsonPropertyAttribute)).Select(att => (string)att.ConstructorArguments[0].Value).First());
            return jsonPropToProp;
        }

    }
}
