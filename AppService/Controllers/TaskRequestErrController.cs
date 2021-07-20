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
    public class TaskRequestErrController : ApiController
    {
        /// <summary>
        /// 入库信息异常查询
        /// </summary>
        /// <param name="TaskRequestErr"></param>
        /// <param name="locNo"></param>
        /// <returns></returns>
        [HttpPost]
        public TaskRequestErr TaskRequesterr([FromBody] TaskRequestErr TaskRequestErr)
        {

            return DBHelper.Instance.TaskRequesterr(TaskRequestErr.storeNo, TaskRequestErr.LocNo, TaskRequestErr.PalletNo, TaskRequestErr.ErrDesc);
        }
    }
}
