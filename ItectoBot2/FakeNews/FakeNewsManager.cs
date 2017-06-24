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
                if (e.Message.Text.Contains(" ") || (!e.Message.Text.Substring(0, 8).Contains("https://") && !e.Message.Text.Substring(0, 7).Contains("http://")))
                    return;
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
                    if (credibility < 20)
                        await e.Channel.SendMessage("Fake news!");
                    else if (credibility < 40)
                        await e.Channel.SendMessage("Tu devrais sérieusement revérifier tes sources.");
                    else if (credibility < 60)
                        await e.Channel.SendMessage("J'ai un doute quand à la véracité de cette information");
                    else if (credibility < 80)
                        await e.Channel.SendMessage("Il y a de forte chance que cela soit vrai");
                    else
                        await e.Channel.SendMessage("Totalement vrai!");
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
