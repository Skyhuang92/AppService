using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class BpalletOutInfo
    {
        //返回任务号
        public string TaskNo   { get; set; }
        //返回托盘号
        public string PalletNo { get; set; }
        //返回库位号
        public string BinNo    { get; set; }
        //库区
        public string StoreNo  { get; set; }
        //手持操作用户名称
        public string UserName { get; set; }

        public string ElocNo { get; set; }
    }
}