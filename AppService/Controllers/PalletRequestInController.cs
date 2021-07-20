using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using AppService.Models;
using AppService.Tools;

namespace AppService.Controllers
{
    public class PalletStackController : ApiController
    {
        /// <summary>
        /// 叠盘-创建agv指令
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        [HttpPost]
        public string StackPalletMethod([FromBody] Bound bound)
        {   
            //获取

            return DBHelper.Instance.LocResidueMaterBack(bound);
        }
    }
}
