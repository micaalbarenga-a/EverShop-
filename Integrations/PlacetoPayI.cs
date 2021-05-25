using Newtonsoft.Json;
using PlacetoPay.Integrations.Library.CSharp.Contracts;
using PlacetoPay.Integrations.Library.CSharp.Entities;
using PlacetoPay.Integrations.Library.CSharp.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Integrations
{
    public class PlacetoPayI
    {
        const string URL_BASE = "https://test.placetopay.com/redirection/";

        const string LOGIN = "6dd490faf9cb87a9862245da41170ff2";
        const string TRANKEY = "024h1IlD";
        
        const string REDIRECT = "https://localhost:44325/app/#!/payed/";


        /// <summary>
        /// Obtiene URL de redirección a la pasarela de pagos.
        /// </summary>
        /// <param name="created"></param>
        /// <param name="totalAmount"></param>
        /// <param name="reference"></param>
        /// <param name="description"></param>
        /// <param name="cusMail"></param>
        /// <param name="cusMobile"></param>
        /// <param name="cusName"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public string GetUrl(DateTime created, double totalAmount, string reference, string description, string cusMail, string cusMobile, string cusName, string userAgent)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Gateway gateway = new PlacetoPay.Integrations.Library.CSharp.PlacetoPay(LOGIN,
                                          TRANKEY,
                                          new Uri(URL_BASE),
                                          Gateway.TP_REST);


                Amount amount = new Amount(totalAmount); //Monto total
                Payment payment = new Payment(reference, description, amount);
                Person person = new Person(string.Empty, string.Empty, cusName, string.Empty, cusMail, mobile: cusMobile);

                RedirectRequest request = new RedirectRequest(payment,
                    REDIRECT + reference,
                    getIP(),
                    userAgent,
                    created.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss"), buyer: person);

                RedirectResponse response = gateway.Request(request);
                
                Environment.SetEnvironmentVariable(reference, response.RequestId);
                Environment.SetEnvironmentVariable("processURL", response.ProcessUrl);

                return response.ProcessUrl;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Obtiene estado de la solicitud de pago a la pasarela 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetStatus(int id)
        {
            try
            {
                Dictionary<string, string> resp = new Dictionary<string, string>();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string requestID = Environment.GetEnvironmentVariable(id.ToString());

                var nonce = (new Random()).GetHashCode().ToString();
                var seed = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                var trankey = Base64(ToSha1(nonce + seed + TRANKEY));

                object auth = new { auth = new { login = LOGIN, tranKey = trankey, nonce = Base64(nonce), seed = (DateTime.Now).ToString("yyyy-MM-ddTHH:mm:sszzz") } };

                var r = JsonConvert.SerializeObject(auth);


                //Llamado a PlacetoPay para obtener el estado del pago
                var http = (HttpWebRequest)WebRequest.Create(new Uri(URL_BASE + "api/session/" + requestID));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";

                string content = r;
                ASCIIEncoding encode = new ASCIIEncoding();
                Byte[] bytes = encode.GetBytes(content);

                Stream str = http.GetRequestStream();
                str.Write(bytes, 0, bytes.Length);
                str.Close();

                var response = http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var cont = sr.ReadToEnd();

                dynamic fullContent = JsonConvert.DeserializeObject(cont);

                string status = fullContent.status.status.ToString();
                string message = fullContent.status.message.ToString();
                resp.Add("status", status);
                resp.Add("message", message);
                resp.Add("processUrl", Environment.GetEnvironmentVariable("processURL"));
                return resp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }




        /// <summary>
        /// Obtención de la ip pública
        /// </summary>
        /// <returns></returns>
        private string getIP()
        {
            return new WebClient().DownloadString("http://api.ipify.org/").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        }


        /*Métodos para generar usuario de autenticación*/

        static public string Base64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        static public string Base64(string input)
        {
            if (input != null)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            }
            return string.Empty;
        }

        static public byte[] ToSha1(string text)
        {
            System.Security.Cryptography.SHA1 hashString = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            return hashString.ComputeHash(ToStream(text));
        }

        static public Stream ToStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(text);
            sw.Flush();
            stream.Position = 0;
            return stream;
        }

    }

}
