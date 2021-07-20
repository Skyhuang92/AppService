using AppService.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AppService.Controllers
{
    public class DownloadController : ApiController
    {

        public HttpResponseMessage GetApkFile()
        {
            try
            {
                var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/apk/android-armv7-debug.apk");
                var stream = new FileStream(FilePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "mesnac.apk"
                };
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
        [HttpGet]
        public string GetVersionMethod()
        {
            return DBHelper.Instance.GetVersionDBMethod();
        }
    }
}
