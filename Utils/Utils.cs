using EverShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverShop.Utils
{
    public class Utils
    {


        /// <summary>
        /// Usuario logueado en la sesión
        /// </summary>
        /// <returns></returns>
        public static User userBySession()
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(HttpContext.Current.Session["id"]);
                return new ShopEntities().Users.Where(u => u.UseId == id).FirstOrDefault();
            }
            catch(Exception e)
            {
                throw e;
            }
        }


    }
}