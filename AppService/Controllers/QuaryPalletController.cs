using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppService.Models;
using AppService.Tools;

namespace AppService.Controllers
{
    public class QuaryPalletController : ApiController
    {
        /// <summary>
        /// 查询托盘基本信息
        /// </summary>
        /// <param name="quaryPallet"></param>
        /// <returns></returns>
        [HttpPost]
        public QuaryPallet QuaryPalletMethod([FromBody] QuaryPallet quaryPallet)
        {

            return DBHelper.Instance.quaryPallet(quaryPallet.PalletNo);
        }
    }
}
