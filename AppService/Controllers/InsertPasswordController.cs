using System;
using AppService.Tools;
using AppService.Models;
using System.Web.Http;

namespace AppService.Controllers
{
    public class InsertPasswordController : ApiController
    {
        [HttpPost]
        public string InsertPassword( [FromBody] InsertPassWord insertData)
        {
            return DBHelper.Instance.insertPassWord(insertData.adminUsername,insertData.adminPassword,insertData.addUsername,insertData.addPassword);
        }

        public string UpdatePassword([FromBody] InsertPassWord insertData)
        {
            return DBHelper.Instance.UpdatePassWord(insertData.adminUsername, insertData.adminPassword, insertData.addUsername, insertData.addPassword);
        }
    }
}