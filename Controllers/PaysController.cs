using EverShop.Models;
using Integrations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EverShop.Controllers
{
    public class PaysController : ApiController
    {

        [HttpGet]
        [Route("api/pay/GetUrl")]
        public string GetUrl(int id, string userAgent)
        {
            try
            {
                //Obtengo url para ir a pasarela de pagos
                ShopEntities entities = new ShopEntities();
                Order order = entities.Orders.Where(o => o.OrdId == id).FirstOrDefault();

                if (order.OrdCreatedAt.AddDays(1) < DateTime.Now) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "La orden ya está vencida"));

                if (order.Product.ProPrice < 1000) throw new Exception("El monto mínimo para proceder a pagar es de $1000");

                User user = Utils.Utils.userBySession();

                //Devolvemos la URL generada
                return new Integrations.PlacetoPayI().GetUrl(order.OrdCreatedAt, (double)order.Product.ProPrice, order.OrdId.ToString(), "Compra de " + order.Product.ProName, user.UseMail, user.UseMobile, user.UseName, userAgent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/pay/GetStatus")]
        public object GetStatus(int id)
        {
            try
            {
                //Actualización de estado de la orden de compra
                ShopEntities entities = new ShopEntities();
                var status = new PlacetoPayI().GetStatus(id);
                string toUpdate = string.Empty;
                bool update = true;

                switch (status["status"].ToUpper())
                {
                    case "APPROVED":
                        toUpdate = "PAYED";
                        status["processUrl"] = null;
                        break;
                    case "REJECTED":
                        toUpdate = "REJECTED";
                        status["processUrl"] = null;
                        break;
                    case "PENDING":
                        update = false;
                        break;
                    default:
                        update = false;
                        status["processUrl"] = null;
                        break;
                }
                if (update)
                {
                    //Si debemos actualizaar el estado de la solicitud: 
                    Order order = entities.Orders.Where(o => o.OrdId == id).First();
                    order.OrdStatus = toUpdate;
                    order.OrdUpdatedAt = DateTime.Now;
                    entities.SaveChanges();
                }

                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
