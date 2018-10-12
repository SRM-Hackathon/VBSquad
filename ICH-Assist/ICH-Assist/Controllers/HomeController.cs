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
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using unirest_net.http;
using OpenTextSummarizer;
using System.IO;

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


    class ApiKeyServiceMicrosoftClientCredentials : ServiceClientCredentials
    {
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", "fef32bb02127432a95c19b51edf88757");
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
        public async Task<ActionResult> Process(string para, string question)
        {
            
            StreamWriter sw1 = new StreamWriter(Server.MapPath(@"~\Content\SampleText.txt"));
            sw1.Flush();
            sw1.Close();
            StreamWriter sw2 = new StreamWriter(Server.MapPath(@"~\Content\SampleText.txt"));
            sw2.Write(para);
            sw2.Close();
            var summarizedDocument = OpenTextSummarizer.Summarizer.Summarize(
                new OpenTextSummarizer.FileContentProvider(Server.MapPath(@"~\Content\SampleText.txt")),
                new SummarizerArguments()
                {
                    Language = "datasets",
                    MaxSummarySentences = 500
                });

            //SummarizedDocument doc = Summarizer.Summarize(sumargs);

            //string summary = string.Join("\r\n\r\n", doc.Sentences.ToArray());
            //string temp = GetAllVerbs(question);
            //string adverbs = GetAllAdverbs(question);
            //string adjust = GetAllAdjective(question);
            //string extract = ExtractSentencesFromString(para);
            string ques_proper_noun = "", ques_noun = "", ques_verb = "";

            string[] sentences = para.Split('.');
            string[] occurances = new string[100];
            //Getting question info
            //string[] verbText = {  "VBN", "VBG", "VBP", "VBZ", "VBD", "VB" };
            //string[] properNounText = {  "VBN", "VBG", "VBP", "VBZ", "VBD", "VB" };
            //string[] nounText = {  "NNPS", "NNP", "NNS","NN" };

            //string ques_adverbs = replaceText(adverbs, verbText);
            //ques_verb = replaceText(temp, verbText);
            //ques_proper_noun= replaceText(GetAllProperNouns(question),properNounText);
            //ques_noun = replaceText(GetAllNouns(question), nounText);


            string[] phrases = getPhrases(question);
            int i = 0;
            foreach (string sentenc in sentences)
            {
                foreach (string phrase in phrases)
                    try
                    {
                        if (sentenc.ToLower().Contains(phrase.ToLower()))
                        {
                            occurances[i++] = sentenc;
                        }
                    }
                    catch (Exception e) { }

            }



            //int i = 0;
            //if (!ques_proper_noun.Equals(""))
            //{

            //        foreach (string sentenc in sentences)
            //        {
            //            if (sentenc.Contains(ques_proper_noun))
            //                occurances[i++] = sentenc;
            //        }


            //}else if (!ques_noun.Equals(""))
            //{

            //        foreach (string sentenc in sentences)
            //        {
            //            if (sentenc.ToLower().Contains(ques_noun.ToLower()))
            //                occurances[i++] = sentenc;
            //        }


            //}
            //Getting para info
            //ViewBag.Message = GetAllVerbs(occurances[j]);
            //ViewBag.Message = GetAllProperNouns(occurances[j]);
            //ViewBag.Message = GetAllNouns(occurances[j]);
            StringBuilder output = new StringBuilder();
            foreach (string sentenc in occurances)
            {
                output = output.Append(sentenc + "\n");

            }

            //Gonna search for its occurance of similar words in paragraph.

            if (!output.ToString().Trim().Equals(""))
            {
                foreach (string phrase in phrases)
                {
                    string[] otherwords = await getSynonyms(phrase);
                    int j = 0;
                    foreach (string sentenc in sentences)
                    {
                        foreach (string word in otherwords)
                            try
                            {
                                if (sentenc.ToLower().Contains(word.ToLower()))
                                {
                                    occurances[j++] = sentenc;
                                }
                            }
                            catch (Exception e) { }

                    }
                    foreach (string sentenc in occurances)
                    {
                        output = output.Append(sentenc + "\n");

                    }

                }



            }


            ViewBag.Message = output;
            return View();
        }
        public string GetAllVerbs(string para)
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
                result = apiInstance.WordsNouns(input);
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
                result = apiInstance.WordsAdjectives(input);
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


        public string[] getPhrases(string para)
        {

            string[] phrases = new string[100];
            // Create a client.
            TextAnalyticsAPI client = new TextAnalyticsAPI(new ApiKeyServiceMicrosoftClientCredentials())
            {
                AzureRegion = AzureRegions.Southeastasia // example 
            };
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          //new MultiLanguageInput("en", "1", "????"),
                          //new MultiLanguageInput("en", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
                          //new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("en", "1", para+".")
                        })).Result;

            // Printing keyphrases
            foreach (var document in result2.Documents)
            {

                int i = 0;
                foreach (string keyphrase in document.KeyPhrases)
                {
                    phrases[i++] = keyphrase;

                }
            }
            return phrases;
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

        public async Task<string[]> getSynonyms(string word)
        {

            string[] otherwords = new string[100];
            using (HttpClient client = new HttpClient())
            {


                using (HttpResponseMessage response = await client.GetAsync("https://api.datamuse.com/words?rel_[syn]=" + word + "&max=4"))
                {
                    using (HttpContent content = response.Content)
                    {
                        string myContent = await content.ReadAsStringAsync();
                        Console.WriteLine(myContent);
                    }
                }
            }
            return otherwords;
        }
    }
}