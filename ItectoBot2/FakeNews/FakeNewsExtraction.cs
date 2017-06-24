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
        public string Icon { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public List<ArticleImage> Images { get; set; }
        public string Html { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Resolved_Url { get; set; }
        public Dictionary<string, string> QueryString { get; set; }
        public List<string> Links { get; set; }
        public int NumPages { get; set; }
        public List<string> Tags { get; set; }
    }
    static class FakeNewsExtraction
    {
        const string Token = "6e121650bbc3a88369784060b046e1bf";

        //Extrait les mots clés de l'article
        public static Dictionary<string, List<string>> Extract(string article)
        {
            //TODO
            string URL = "https://api.diffbot.com/v2/article?token=" + Token + "&url=" + article;
            
            WebRequest request = WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            string jsonText;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonText = sr.ReadToEnd();
            }
           

            ArticleContainer deserializedProduct = JsonConvert.DeserializeObject<ArticleContainer>(jsonText);
            Console.WriteLine(deserializedProduct.Author);

            return new Dictionary<string, List<string>>();
        }
    }
    public class ArticleImage
    {
        public bool Primary { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public int PixelHeight { get; set; }
        public int PixelWidth { get; set; }
    }
    public class ArticleVideo
    {
        public bool Primary { get; set; }
        public string Url { get; set; }
        public int PixelHeight { get; set; }
        public int PixelWidth { get; set; }
    }

}
