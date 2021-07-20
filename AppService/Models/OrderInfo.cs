using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class OrderInfo
    {
        public string ORDER_NO { get; set; }
        public string ORDER_STATUS { get; set; }
        public string ORDER_TYPE_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
    }
}