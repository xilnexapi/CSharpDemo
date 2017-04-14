using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RestAPIExample.Controllers
{
    public class CustomerController : ApiController
    {
        // GET api/Customer
        public HttpResponseMessage Get()
        {
            Dictionary<string, string> dcHeaders = new Dictionary<string, string>();
            dcHeaders.Add("dkey", ""); // Request from Xilnex api team
            dcHeaders.Add("token", ""); // Request from Xilnex api team
            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = RequestInResponseMessage("https://api.xilnex.com/admin/v1/customers/count", dcHeaders, "", RequestType.Get);
            }
            catch(Exception ex)
            {
                httpResponseMessage = new HttpResponseMessage();
                httpResponseMessage.StatusCode = HttpStatusCode.BadRequest;
                httpResponseMessage.Content = new StringContent(ex.Message);
                
            }
            return httpResponseMessage;
        }

        public enum RequestType { Get, Post, Put, Delete }

        public HttpResponseMessage RequestInResponseMessage(string url, Dictionary<string, string> header, string body, RequestType requestType)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            if (string.IsNullOrEmpty(body))
                body = "";

            string response_json = "";
            HttpClient client = new HttpClient();

            if (header != null)
            {
                foreach (string key in header.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, header[key]);
                }
            }

            using (HttpContent content = new StringContent(body, Encoding.UTF8, "application/json"))
            {
                if (requestType == RequestType.Post)
                {
                    using (HttpResponseMessage response = client.PostAsync(url, content).Result)
                    {
                        response_json = response.Content.ReadAsStringAsync().Result;
                        httpResponseMessage.StatusCode = response.StatusCode;
                        httpResponseMessage.Content = new StringContent(response_json, System.Text.Encoding.UTF8, "application/json");
                    }
                }
                else if (requestType == RequestType.Put)
                {
                    using (HttpResponseMessage response = client.PutAsync(url, content).Result)
                    {
                        response_json = response.Content.ReadAsStringAsync().Result;
                        httpResponseMessage.StatusCode = response.StatusCode;
                        httpResponseMessage.Content = new StringContent(response_json, System.Text.Encoding.UTF8, "application/json");
                    }
                }
                else if (requestType == RequestType.Delete)
                {
                    using (HttpResponseMessage response = client.DeleteAsync(url).Result)
                    {
                        response_json = response.Content.ReadAsStringAsync().Result;
                        httpResponseMessage.StatusCode = response.StatusCode;
                        httpResponseMessage.Content = new StringContent(response_json, System.Text.Encoding.UTF8, "application/json");
                    }
                }
                else if (requestType == RequestType.Get)
                {
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        response_json = response.Content.ReadAsStringAsync().Result;
                        httpResponseMessage.StatusCode = response.StatusCode;
                        httpResponseMessage.Content = new StringContent(response_json, System.Text.Encoding.UTF8, "application/json");
                    }
                }
            }

            return httpResponseMessage;
        }
    }
}
