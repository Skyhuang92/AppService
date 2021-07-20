using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class SortingResult
    {
        public string ErrCode { get; set; }
        public string ErrDesc { get; set; }
        public string SortingLocNo { get; set; }
        public string ScanTime { get; set; }
        public string ProductId { get; set; }
        public string Barcode { get; set; }
        public string PscanNo { get; set; }
    }
}