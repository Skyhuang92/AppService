using AppService.Models;
using AppService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppService.Controllers
{
    public class BpalletOutOrderController : ApiController
    {
        [HttpPost]
        public BpalletOutInfo GetBpalletOut([FromBody] BpalletOutInfo locNo) {
            
                return DBHelper.Instance.GetOutBpalletInfoFromDB(locNo.BinNo, locNo.StoreNo, locNo.UserName,locNo.ElocNo);
        }
    }
}
