using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;


namespace ItectoBot2.FakeNews
{
    class ArticleContainer
    {
        public string type;
        public string title;
        public string text;
        public string html;
        public string date;
        public string estimateDate;
        public string author;
        public string authorUrl;
        public string discussion;
        public string humanLanguage;
        public string numPages;
        public string nextPages;
        public string siteName;
        public string publisherRegion;
        public string publisherCountry;
        public string pageUrl;
        public string resolvedPageUrl;
        public string[] tags;
        public string[] images;
        public string[] videos;
        public string[] breadcrumb;
        public string diffbotUri;
    }
    static class FakeNewsExtraction
    {
        const string Token = "6e121650bbc3a88369784060b046e1bf";

        //Extrait les mots clés de l'article
        public static Dictionary<string, List<string>> Extract(string article)
        {
            //TODO
            string URL = "https://api.diffbot.com/v3/article?token=" + Token + "&url=" + article;

            WebRequest request = WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            string jsonText;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonText = sr.ReadToEnd();
            }
            Console.WriteLine(jsonText);

            ArticleContainer deserializedProduct = JsonConvert.DeserializeObject<ArticleContainer>(jsonText);
            Console.WriteLine(deserializedProduct.ToString());
            Console.WriteLine(deserializedProduct.siteName);
            Console.WriteLine(deserializedProduct.text);
            Console.WriteLine(deserializedProduct.type);

            return new Dictionary<string, List<string>>();
        }
    }
}
