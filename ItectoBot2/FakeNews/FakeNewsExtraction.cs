
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
    static class FakeNewsExtraction
    {
        const string Token = "6e121650bbc3a88369784060b046e1bf";

        //Extrait les mots clés de l'article
        public static Dictionary<string, List<string>> Extract(string article)
        {
            Dictionary<string, List<string>> infos = new Dictionary<string, List<string>>();


            JObject jo = JObject.Parse(FakeNewsManager.GetJsonFromURL("https://api.diffbot.com/v3/article?&fields=links,meta&token=" + Token + "&url=" + article));
            string s = jo.SelectToken("objects[0].siteName").ToString();
            string s2 = s.Replace(" ", "").ToLower();
            infos.Add("website", new List<string>() { s2 });

            JToken title = jo.SelectToken("objects[0].title");
            infos.Add("title", new List<string>() { title == null ? "" : title.ToString() });
            JToken author = jo.SelectToken("objects[0].author");
            infos.Add("author", new List<string>() { author == null ? "" : author.ToString() });
            JToken date = jo.SelectToken("objects[0].date");
            infos.Add("date", new List<string>() { date == null ? "" : date.ToString() });
            return infos;
        }

    }
}