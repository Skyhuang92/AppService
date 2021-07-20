using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class PalletBoundInput
    {
        /// <summary>
        /// 站台号
        /// </summary>
        public string LocNo { get; set; }
        /// <summary>
        /// 托盘号
        /// </summary>
        public string PalletNo { get; set; }
      /// <summary>
      /// 录入人
      /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 托盘类型
        /// </summary>
        public string PalletType { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string MaterNo { get; set; }


    }
}