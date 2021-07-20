using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class PalletBound
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public string productGuid { get; set; }
        public string storeNo { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        public string materNo { get; set; }
        //异常描述
        public string palletNo { get; set; }
        //手持操作用户名
        public string UserName { get; set; }
    }
}