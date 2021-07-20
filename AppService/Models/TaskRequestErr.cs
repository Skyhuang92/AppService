using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class TaskRequestErr
    {
        public string PalletNo { get; set; }
        public string storeNo { get; set; }
        public string LocNo { get; set; }
        //异常描述
        public string ErrDesc { get; set; }
        //手持操作用户名
        public string UserName { get; set; }
    }
}