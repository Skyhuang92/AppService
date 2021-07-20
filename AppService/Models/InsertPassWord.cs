using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService.Models
{
    public class InsertPassWord
    {
        public string adminUsername { get; set; }
        public string adminPassword { get; set; }
        public string addUsername { get; set; }
        public string addPassword { get; set; }
    }
}