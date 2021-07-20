using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppService.Controllers
{
    public class CategoriesController : ApiController
    {
        /// <summary>
        /// home图标显示
        /// </summary>
        Categories[] categories = new Categories[]
        {
            new Categories { Id = 1, FavoritesTitle="物料绑定",Icon="icon-binding",IsEnable=1 },
            new Categories { Id = 2, FavoritesTitle="生产入库",Icon="icon-fahuo" ,IsEnable=1 },
            //new Categories { Id = 3, FavoritesTitle="托盘查询",Icon="icon-chaxun" ,IsEnable=1 },
            //new Categories { Id = 4, FavoritesTitle="库位出库",Icon="icon-chuku"  ,IsEnable=1 },
            //new Categories { Id = 5, FavoritesTitle="扫码发货",Icon="icon-fahuo"  ,IsEnable=1 },
            //new Categories { Id = 6, FavoritesTitle="空盘出库",Icon="icon-chuku"  ,IsEnable=1 },
            new Categories { Id = 7, FavoritesTitle="余料回库",Icon="icon-chuku"  ,IsEnable=1 },
            //new Categories { Id = 8, FavoritesTitle="入库异常",Icon="icon-binding"  ,IsEnable=1 },
            //new Categories { Id = 9, FavoritesTitle="托盘入库",Icon="icon-binding"  ,IsEnable=1 }
        };
        [HttpGet]
        public IEnumerable<Categories> GetAllProducts()
        {
            return categories;
        }
        [HttpGet]
        public IHttpActionResult GetProduct(int id)
        {
            var category = categories.FirstOrDefault((p) => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        public string Options()
        {
            return null;
        }
    }
}

