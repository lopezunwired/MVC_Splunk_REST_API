using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace SplunkCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<SplunkConfig> config;
        private readonly string SPLUNKRESTAPIURL = string.Empty;
        private readonly string UserName = string.Empty;
        private readonly string Password = string.Empty;
        private readonly object JsonRequestBehavior;

        public HomeController(IOptions<SplunkConfig> config)
        {
            this.config = config;
           this.SPLUNKRESTAPIURL= config.Value.SplunkRestApiUrl;
            this.UserName = config.Value.UserName;
            this.Password = config.Value.Password;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAuthenticate()
        {
            string Key = string.Empty;
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var _client = new HttpClient(handler))
                {
                    try
                    {
                        _client.BaseAddress = new Uri(SPLUNKRESTAPIURL);
                        var request = new HttpRequestMessage(HttpMethod.Post, "/services/auth/login");
                        request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("username", UserName),
                        new KeyValuePair<string, string>("password", Password)});
                        var _result1 = Task.Run(async () => await _client.SendAsync(request)).Result;
                        string __result = _result1.Content.ReadAsStringAsync().Result;
                        XElement incomingXml = XElement.Parse(__result);
                        string key = (string)incomingXml.Element("sessionKey");
                        Key = key;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            var _dic = new Dictionary<string, string>();
            _dic.Add("SessionKey", Key);
            return _dic;
        }
        [HttpGet]
        public JsonResult GetData(string sessionkey)
        {
            string __result = string.Empty;
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var _client = new HttpClient(handler))
                {
                    try
                    {
                        _client.BaseAddress = new Uri(SPLUNKRESTAPIURL);
                        var request = new HttpRequestMessage(HttpMethod.Get, "/servicesNS/admin/search/data/lookup-table-files?output_mode=json");
                        request.Headers.Add("Authorization",string.Format("Splunk {0}", sessionkey));
                        var _result1 = Task.Run(async () => await _client.SendAsync(request)).Result;
                        __result = _result1.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return Json(__result);
        }
    }
}
