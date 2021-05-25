
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
                //Para los casos de usuarios admin, mostramos todos las órdenes
                Models.ShopEntities entities = new Models.ShopEntities();

                User user = null;
                try
                {
                    user = Utils.Utils.userBySession();
                }
                catch (Exception)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Sesión expirada"));
                }

                List<object> ret = new List<object>();
                if (user.UseAdmin)
                {
                    var orders = entities.Orders.ToList();
                    foreach (var orden in orders)
                    {
                        if (orden.OrdCreatedAt.AddDays(1) < DateTime.Now)
                            orden.OrdStatus = "Vencida";
                        ret.Add(new { Order = orden, Product = entities.Products.Where(p => p.ProId == orden.OrdProduct).FirstOrDefault() });
                    }
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
                //Obtengo todas las órdenes pendientes del usuario que está logueado
                Models.ShopEntities entities = new Models.ShopEntities();
                User user = null;
                try
                {
                    user = Utils.Utils.userBySession();
                }
                catch (Exception e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Sesión expirada"));
                }
                return entities.Orders.Where(o => user.UseId == o.OrdUser).ToList(); ;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        [HttpPost]
        [Route("api/order/Orders")]
        public async Task<IHttpActionResult> AddOrder([FromBody] JObject product)
        {
            if (ModelState.IsValid)
            {
                ShopEntities entities = new ShopEntities();

                User user = Utils.Utils.userBySession();

                //Obtengo id de producto del objeto recibido por POST
                int idProd = Convert.ToInt32(product["Product"]["ProId"]);

                Order order = new Order { OrdProduct = idProd, OrdUser = user.UseId, OrdCreatedAt = DateTime.Now, OrdStatus = "CREATED" };

                //Alta de la orden
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
