using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class TyreInStorage
    {
        public int BAT_QTY { get; set; }
        public int EACH_BAT_QTY { get; set; }
        public string PalletNo { get; set; }
        public string Barcode { get; set; }
        public string PRODUCT_ID { get; set; }
        public string PRODUCT_GUID { get; set; }
        public string STACK_IDX { get; set; }
        public string LAYRE_IDX { get; set; }
    }
    public class ProductsIDList
    {
        public List<List<TyreInStorage>> TyreList { get; set; }
        public string PalletNo { get; set; }
        public string LocNo { get; set; }
    }

}