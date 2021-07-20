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
    public class SendGoodsForPalletController : ApiController
    {
        [HttpPost]
        public string[] SendGoodsForPalletMethod([FromBody] SendGoods paramInfo)
        {

            return DBHelper.Instance.SendGoodsForPalletDBMethod(paramInfo.PalletNo,paramInfo.Barcode,paramInfo.UserName);

        }
    }
}
