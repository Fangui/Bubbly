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
    static class FakeNewsManager
    {
        const string Token = "6e121650bbc3a88369784060b046e1bf";
        public static void Start()
        {
            Program.client.MessageReceived += Client_MessageReceived;
        }

        private async static void Client_MessageReceived(object sender, Discord.MessageEventArgs e)
        {
            if (!e.User.IsBot && e.Message.Text.Length > 0)
            {
                string URL = "https://api.diffbot.com/v3/analyze?token=" + Token + "&url=" + e.Message.Text;

                WebRequest request = WebRequest.Create(URL);
                request.Method = "GET";
                request.ContentType = "application/json; charset=utf-8";

                var response = (HttpWebResponse)request.GetResponse();
                string jsonText;
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    jsonText = sr.ReadToEnd();
                }

                Container deserializedProduct = JsonConvert.DeserializeObject<Container>(jsonText);
                if(deserializedProduct.title == "Article")
                {
                    float credibility = FakeNewsProbability.GetProbability(FakeNewsExtraction.Extract(e.Message.Text));
                }
            }
        }
    }
    class Container
    {
        public string title;
        public string type;
        public string humanLanguage;
        //Optional
        string links;
    }
}
