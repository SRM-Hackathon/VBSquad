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
using System.Text;

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
        public ActionResult Process(string para,string question)
        {


            string temp = GetAllVerbs(question);
            string adverbs = GetAllAdverbs(question);
            string adjust = GetAllAdjective(question);
            string extract = ExtractSentencesFromString(para);
            string ques_proper_noun = "", ques_noun = "", ques_verb = "";

            string[] sentences = para.Split('.');
            string[] occurances = { };
            //Getting question info
            string[] verbText = {  "VBN", "VBG", "VBP", "VBZ", "VBD", "VB" };
            string[] properNounText = {  "VBN", "VBG", "VBP", "VBZ", "VBD", "VB" };
            string[] nounText = {  "VBN", "VBG", "VBP", "VBZ", "VBD", "VB" };

            string ques_adverbs = replaceText(adverbs, verbText);
            ques_verb = replaceText(temp, verbText);
            ques_proper_noun= replaceText(GetAllProperNouns(question),properNounText);
            ques_noun = replaceText(GetAllNouns(question), nounText);
            ViewBag.Message = GetAllVerbs(question);
            ViewBag.Message = GetAllProperNouns(question);
            ViewBag.Message = GetAllNouns(question);


            int i = 0;
            if (!ques_proper_noun.Equals(""))
            {
               
                    foreach (string sentenc in sentences)
                    {
                        if (sentenc.Contains(ques_proper_noun))
                            occurances[i++] = sentenc;
                    }
               
                
            }
            //Getting para info
            //ViewBag.Message = GetAllVerbs(occurances[j]);
            //ViewBag.Message = GetAllProperNouns(occurances[j]);
            //ViewBag.Message = GetAllNouns(occurances[j]);

            return View();
        }
        public  string GetAllVerbs(string para)
        {
            
            //api key
            //29809699 - a20d - 448a - 85a8 - 83956f239c00
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
            var apiInstance = new WordsApi();
            var input = para;  // string | Input string

            string result = "";
            try
            {
                // Get the verbs in a string
                result = apiInstance.WordsPost(input);
                Debug.WriteLine(result);
                
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WordsApi.WordsPost: " + e.Message);
            }
            return result;
        }
        public string GetAllProperNouns(string para)
        {

            
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
            
            var apiInstance = new WordsApi();
            var input = para;  // string | Input string
            string result = "";
            try
            {
                // Get proper nouns in a string
                result = apiInstance.WordsProperNouns(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WordsApi.WordsProperNouns: " + e.Message);
            }
            
            return result;
        }
        public string GetAllNouns(string para)
        {

            //api key
            //29809699 - a20d - 448a - 85a8 - 83956f239c00
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
            var apiInstance = new WordsApi();
            var input = para;  // string | Input string
            string result = "";
            try
            {
                // Get nouns in string
                result=apiInstance.WordsNouns(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WordsApi.WordsNouns: " + e.Message);
            }
            return result;
        }

        public string GetAllAdverbs(string para)
        {

            //api key
            //29809699 - a20d - 448a - 85a8 - 83956f239c00
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
             var apiInstance = new WordsApi();
            var input = para;  // string | Input string
            string result = "";

            try
            {
                // Get adverbs in input string
                result = apiInstance.WordsAdverbs(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WordsApi.WordsAdverbs: " + e.Message);
            }
            return result;
        }

        public string GetAllAdjective(string para)
        {

            //api key
            //29809699 - a20d - 448a - 85a8 - 83956f239c00
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
            string result = "";

            var apiInstance = new WordsApi();
            var input = para;  // string | Input string

            try
            {
                // Get adjectives in string
               result=apiInstance.WordsAdjectives(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WordsApi.WordsAdjectives: " + e.Message);
            }
            return result;
        }

        public string ExtractSentencesFromString(string para)
        {

            //api key
            //29809699 - a20d - 448a - 85a8 - 83956f239c00
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "29809699-a20d-448a-85a8-83956f239c00");
            string result = "";
            var apiInstance = new SentencesApi();
            var input = para;  // string | Input string

            try
            {
                // Extract sentences from string
                result = apiInstance.SentencesPost(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling SentencesApi.SentencesPost: " + e.Message);
            }
            return result;
        }


        public string replaceText(string sentence, string[] unwanted)
        {
            
            StringBuilder builder = new StringBuilder(sentence);
            foreach (string unwant in unwanted)
            {

                builder.Replace(unwant, "");

            }
            builder.Replace(@"\", "");
            builder.Replace("/", "");
            builder.Replace("\"", "");
            return builder.ToString();
        }
    }
}