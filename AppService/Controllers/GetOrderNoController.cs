using AppService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppService.Controllers
{
    public class GetOrderNoController : ApiController
    {
        [HttpGet]
        public string[] GetOrderNoMethod()
        {
           return DBHelper.Instance.GetOrderNoFromDB();
        }
    }
}
