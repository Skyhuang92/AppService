using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    /// <summary>
    /// 信息准备类
    /// </summary>
    public class PrepareInfo
    {
        /// <summary>
        /// 物料类型数组
        /// </summary>
        public string[][] materTypeArr { get; set; }
        /// <summary>
        /// 托盘类型数组
        /// </summary>
        public string[][] palletTypeArr { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        public string[][] materNoArr { get; set; }       
    } 
}