using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloudmersive.APIClient.NET.NLP.Api;
using Cloudmersive.APIClient.NET.NLP.Client;
using Cloudmersive.APIClient.NET.NLP.Model;
using System.Diagnostics;

namespace ICH_Assist.Controllers
{
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
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling PosTaggerStringApi.PosTaggerStringPost: " + e.Message);
            }
            return View();
        }
    }
}