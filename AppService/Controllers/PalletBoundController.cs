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
    public class PalletBoundController : ApiController
    {
        /// <summary>
        /// 入库前查询托盘绑定信息
        /// </summary>
        /// <param name="PalletBound"></param>
        /// <returns></returns>
        [HttpPost]
        public PalletBound PalletBound([FromBody] PalletBound PalletBound)
        {
            return DBHelper.Instance.PalletBound(PalletBound.palletNo);
        }
    }
}
