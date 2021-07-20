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
    public class StorageOutController : ApiController
    {
        [HttpPost]
        public StorageOutInfo GetOutStorageInfo([FromBody] StorageOutInfo binNo) {
            
                return DBHelper.Instance.GetOutStorageInfoFromDB(binNo.BinNo, binNo.StoreNo,binNo.UserName);
        }
    }
}
