using EverShop.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EverShop.Controllers
{
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("api/login/login")]
        public bool Login([FromBody] JObject data)
        {
            string mail = data["mail"].ToString();
            string pass = data["pass"].ToString();
            ShopEntities ent = new ShopEntities();
            User user = ent.Users.Where(u => mail == u.UseMail && u.UsePassword == pass).FirstOrDefault();

            if (user is null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "El usuario o la clave son incorrectos"));
            }
            HttpContext context = HttpContext.Current;
            context.Session["id"] = user.UseId;
            context.Session["User"] = user;

            return user.UseAdmin;

            
        }




        [HttpPost]
        [Route("api/login/logout")]
        public void Logout()
        {
            HttpContext.Current.Session["id"] = null;
            HttpContext.Current.Session["User"] = null;

        }

    }
}
