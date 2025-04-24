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
    public class LmsChartController : ApiController
    {
        [HttpGet, HttpOptions, EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("LmsChart/{sourceKey}/{segmentKey}")]
        public async Task<HttpResponseMessage> LmsChart([FromUri]string sourceKey, [FromUri] string segmentKey, string measurements , int height = 300, int width = 500)
        {
            var chartSettings = new LmsChartSettings()
            {
                SourceKey = sourceKey,
                SegmentKey = segmentKey,
                Height = height,
                Width = width,
                ShowAxis = true,
                ZoomMode = EZoomMode.FocusedAndNearby,
                FontSize = 10f, //SystemFonts.DefaultFont.Size,
            };

            var chartData = JsonConvert.DeserializeObject<List<LmsMeasurement>>(measurements);

            var chart = new EasyChart();
            var image = chart.GenerateLmsChart(chartSettings, chartData);

            return GetImageResponse(image);
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
