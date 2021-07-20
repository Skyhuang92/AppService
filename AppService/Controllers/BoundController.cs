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
    public class BoundController : ApiController
    {
        

        /// <summary>
        /// 物料绑定
        /// </summary>
        /// <param name="BoundList"></param>
        /// <returns></returns>
        [HttpPost]
        public string BoundingMethod([FromBody] Bound bound)
        {
            string result = DBHelper.Instance.BoundingMethod(bound);
            if (string.IsNullOrEmpty(result))
            {
                return "物料绑定成功";
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 物料大类、托盘类型查询
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        [HttpPost]
        public PrepareInfo QueryPrepareInfoMethod()
        {
            PrepareInfo info = new PrepareInfo();
            //查询物料类型数组
            info.materTypeArr = DBHelper.Instance.MaterTypeListQuery();
            //查询托盘类型数组
            info.palletTypeArr = DBHelper.Instance.PalletTypeListQuery();

            return info;
        }

        /// <summary>
        /// 物料类型名称查询
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        [HttpPost]
        public PrepareInfo QueryMaterNoArrMethod([FromBody] Bound bound)
        {
            PrepareInfo info = new PrepareInfo();
            //查询物料编号数组
            info.materNoArr = DBHelper.Instance.MaterNoArrQuery(bound.MaterType);           

            return info;
        }

        /// <summary>
        /// 物料绑定
        /// </summary>
        /// <param name="BoundList"></param>
        /// <returns></returns>
        [HttpPost]
        public string BoundInputMethod([FromBody] Bound bound)
        {
            string result = DBHelper.Instance.BoundingMethod(bound);
            if (string.IsNullOrEmpty(result))
            {
                result = DBHelper.Instance.StoreInputMethod(bound);
                if(string.IsNullOrEmpty(result))
                {
                    return "物料绑定,生成入库任务成功";
                }
                else
                {
                    return "物料绑定成功,入库任务生成失败:" + result;
                }
                
            }
            else
            {
                return result;
            }
        }
    }
}
