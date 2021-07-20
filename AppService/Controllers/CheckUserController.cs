using AppService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppService.Controllers
{
    public class CheckUserController : ApiController
    {
        /// <summary>
        /// 用户登陆验证
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public int CheckResult([FromBody] LoginEntity login)
        {
            try
            {
                if (DBHelper.Instance.GetResultFromDB(login.UserName, login.PassWord))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
  
        public class LoginEntity
        {
            public string UserName { get; set; }
            public string PassWord { get; set; }
        }
        public string Options()
        {
            return null;
        }
    }
}
