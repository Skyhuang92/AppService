using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class SendGoods
    {
        public int OrderSendCount { get; set; }
        public int HasSendCount { get; set; }
        public string SendDesc { get; set; }
        public string Barcode { get; set; }
        public string OrderNo { get; set; }
        public string PalletNo { get; set; }
        public string UserName { get; set; }
    }
}