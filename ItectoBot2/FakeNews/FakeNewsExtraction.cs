using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

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
            string URL = "https://api.diffbot.com/v3/article?&fields=links,meta&token=" + Token + "&url=" + article;
            
            WebRequest request = WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            string jsonText;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonText = sr.ReadToEnd();
            }

            //Console.WriteLine(jsonText);
            ArticleContainer deserializedProduct = JsonConvert.DeserializeObject<ArticleContainer>(jsonText);
            Console.WriteLine(deserializedProduct.Author);
            //deserializedProduct.Author= deserializedProduct.Author.ToLower();
            //deserializedProduct.Author = deserializedProduct.Author.Remove(' ');

            /*Dictionary<string, object> jSonBase = deserializeToDictionary(jsonText);
            Console.WriteLine(((JArray)(jSonBase["objects"])).SelectToken("object[0].author.text").ToString());*/

            JObject jo = JObject.Parse(jsonText);
            Console.WriteLine(jo.SelectToken("objects[0].author"));

            return new Dictionary<string, List<string>>();
        }

        static private Dictionary<string, object> deserializeToDictionary(string jo)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
            var values2 = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> d in values)
            {
                // if (d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject"))
                if (d.Value is JObject)
                {
                    values2.Add(d.Key, deserializeToDictionary(d.Value.ToString()));
                }
                else
                {
                    values2.Add(d.Key, d.Value);
                }
            }
            return values2;
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
