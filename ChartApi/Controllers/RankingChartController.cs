using ChartApi.Utils;
using System;
using System.Collections.Generic;
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
        [HttpGet, HttpPost, HttpOptions, EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<int?> Single(string template = "default")
        {
            var list = new List<int?> { null, 1, 2, 3, null };

            return list;

            //var templateManager = new ChartTemplateManager();
            //var settings = templateManager.Load(template);

            //var queryParams = Request.GetQueryNameValuePairs().ToDictionary(item => item.Key, item => item.Value);
            //queryParams.Remove("chart_template");


            //return queryParams;
        }


        public HttpResponseMessage GetFile()
        {
            var bmp = new System.Drawing.Bitmap(40, 30);
            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
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
