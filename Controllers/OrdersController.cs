
using EverShop.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EverShop.Controllers
{
    public class OrdersController : ApiController
    {

        private Models.ShopEntities db = new Models.ShopEntities();


        [HttpGet]
        [Route("api/order/AllOrders")]
        public IEnumerable<object> GetAll()
        {
            try
            {
                Models.ShopEntities entities = new Models.ShopEntities();
                var orders = entities.Orders.ToList();
                List<object> ret = new List<object>();
                foreach (var orden in orders)
                {
                    if (orden.OrdCreatedAt.AddDays(1) < DateTime.Now)
                        orden.OrdStatus = "Vencida";
                    ret.Add(new { Order = orden, Product = entities.Products.Where(p => p.ProId == orden.OrdProduct).FirstOrDefault() });
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("api/order/MyOrders")]
        public IEnumerable<Order> Get()
        {
            try
            {
                Models.ShopEntities entities = new Models.ShopEntities();
                int id = 0;
                try
                {
                    id = Convert.ToInt32(HttpContext.Current.Session["id"]);
                }
                catch
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Sesión expirada"));
                }

                return entities.Orders.Where(o => id == o.OrdUser).ToList(); ;
            }
            catch (Exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Sesión expirada"));
            }

        }


        [HttpPost]
        [Route("api/order/Orders")]
        public async Task<IHttpActionResult> AddOrder([FromBody] JObject product)
        {
            if (ModelState.IsValid)
            {
                ShopEntities entities = new ShopEntities();
                int usId = Convert.ToInt32(HttpContext.Current.Session["id"]);
                User user = entities.Users.Where(u => u.UseId == usId).FirstOrDefault();

                int idProd = Convert.ToInt32(product["Product"]["ProId"]);

                Order order = new Order { OrdProduct = idProd, OrdUser = usId, OrdCreatedAt = DateTime.Now, OrdStatus = "CREATED" };
                db.Orders.Add(order);
                await db.SaveChangesAsync();

                Product prod = entities.Products.Where(p => idProd == p.ProId).FirstOrDefault();
                order.Product = prod;
                return Ok(order);
            }
            else
            {
                return BadRequest();
            }
        }




    }
}
