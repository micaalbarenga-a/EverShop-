using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EverShop.Controllers
{
    public class ProductsController : ApiController
    {
        [HttpGet]
        [Route("api/product/Products")]
        public IEnumerable<Models.Product> Get()
        {
            try
            {
                //Listado de productos disponibles para comprar

                Models.ShopEntities entities = new Models.ShopEntities();
                return entities.Products.OrderBy(o => o.ProName).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
