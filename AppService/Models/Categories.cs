using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppService
{
    public class Categories
    {
        /// <summary>
        /// 界面图标属性
        /// </summary>
        public int Id { get; set; }
        public string FavoritesTitle { get; set; }
        public string Icon { get; set; }
        public int IsEnable { get; set; }
    }
}