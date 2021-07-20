using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using AppService.Models;
using AppService.Tools;

namespace AppService.Controllers
{
    /// <summary>
    /// 物料回库页面功能实现
    /// </summary>
    public class MaterBackController : ApiController
    {
        /// <summary>
        /// 区域类型查询
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        [HttpPost]
        public Dictionary<string,string> QueryBackLocAreaMethod()
        {           
            return DBHelper.Instance.BackLocAreaQuery();
        }

        /// <summary>
        /// 区域类型查询
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        [HttpPost]
        public string LocResidueMaterBackMethod([FromBody] Bound bound)
        {
            //数量核查
            string pattern = @"^\d*$";
            if(!Regex.IsMatch(bound.ProductQty, pattern))
            {
                return "请录入正确数量信息";
            }
            
            return DBHelper.Instance.LocResidueMaterBack(bound);
        }
    }
}