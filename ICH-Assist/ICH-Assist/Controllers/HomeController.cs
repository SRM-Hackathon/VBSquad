using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloudmersive.APIClient.NET.NLP.Api;
using Cloudmersive.APIClient.NET.NLP.Client;
using Cloudmersive.APIClient.NET.NLP.Model;
using System.Diagnostics;
//using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Rest;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;

namespace ICH_Assist.Controllers
{
    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", "28be2df0a9b047258b0e86a623d48fa1");
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Process(string para)
        {
            //api key
            //29809699 - a20d - 448a - 85a8 - 83956f239c00
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
            var apiInstance = new PosTaggerStringApi();
            var input = para;  // string | Input string


            try
            {
                // Part-of-speech tag a string
                string result = apiInstance.PosTaggerStringPost(input);
                Debug.WriteLine(result);
                ViewBag.Message = result;
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PosTaggerStringApi.PosTaggerStringPost: " + e.Message);
            }
            return View();
        }
    }
}