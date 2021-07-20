using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class Bound
    {
        /// <summary>
        /// 物料小类
        /// </summary>
        public string MaterType { get; set; }
        /// <summary>
        /// 托盘类型 吨包 / 小袋
        /// </summary>
        public string PalletType { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterNo { get; set; }
        /// <summary>
        /// 物料名称 MATER_NAME
        /// </summary>
        public string MaterName { get; set; }
        /// <summary>
        /// 托盘(工装)编号
        /// </summary>
        public string PalletNo { get; set; }      
        /// <summary>
        /// 录入人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 产品重量(吨)/托盘数量(个)
        /// </summary>
        public string ProductQty { get; set; }
        /// <summary>
        /// 信息绑定机台
        /// </summary>
        public string LocNo { get; set; }
        /// <summary>
        /// 线体编号
        /// </summary>
        public string LineNo { get; set; }
        /// <summary>
        /// 架次
        /// </summary>
        public string RackNo { get; set; }
        /// <summary>
        /// 班组
        /// </summary>
        public string ShiftNo { get; set; }
    }
}