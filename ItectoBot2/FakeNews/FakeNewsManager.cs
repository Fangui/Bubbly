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
    static class FakeNewsManager
    {
        const string Token = "6e121650bbc3a88369784060b046e1bf";
        public static void Start()
        {
            Program.client.MessageReceived += Client_MessageReceived;
        }

        private async static void Client_MessageReceived(object sender, Discord.MessageEventArgs e)
        {
            if (!e.User.IsBot && e.Message.Text.Length > 7)
            {
                if (e.Message.Text.Contains(" ") || (!e.Message.Text.Substring(0, 8).Contains("https://") && !e.Message.Text.Substring(0, 7).Contains("http://")))
                    return;
                if (e.Message.Text.Contains("www.facebook.com"))
                {
                    List<string> articles = new List<string>();
                    string jsonText = GetJsonFromURL("https://api.diffbot.com/v3/article?token=6e121650bbc3a88369784060b046e1bf&fields=links&url=" + e.Message.Text);
                    JObject jo = JObject.Parse(jsonText);
                    string[] links = jo.SelectToken("objects[0].links").ToString().Trim('[', ']', ' ').Split(',');
                    foreach (string link in links)
                    {
                        if (link.Contains("l.facebook.com/l.php?u="))
                        {
                            int startIndex = link.IndexOf('=') + 1;
                            int endIndex = link.IndexOf('=', startIndex);
                            await SendAvis(e, link.Substring(startIndex, endIndex - startIndex));
                            return;
                        }
                    }
                }
                else if (e.Message.Text.Contains("www.youtube.com"))
                {
                    await e.Channel.SendMessage("TODO_YOUTUBE_LUKA_FUNCTION");
                }
                else
                {
                    await SendAvis(e, e.Message.Text);
                }
            }
        }
        static async Task SendAvis(Discord.MessageEventArgs e, string article)
        {
            string URL = "https://api.diffbot.com/v2/analyze?token=" + Token + "&url=" + article;

            Container deserializedProduct = JsonConvert.DeserializeObject<Container>(GetJsonFromURL(URL));
            if (deserializedProduct.type == "article")
            {
                float credibility = FakeNewsProbability.GetProbability(FakeNewsExtraction.Extract(e.Message.Text));
                if (credibility < 20)
                    await e.Channel.SendMessage("**" + deserializedProduct.title + "** est une fake news!");
                else if (credibility < 40)
                    await e.Channel.SendMessage("**" + deserializedProduct.title + "** est surement une fake news.");
                else if (credibility < 60)
                    await e.Channel.SendMessage("**" + deserializedProduct.title + "** m'inspire peu confiance. Tu devrais aller vérifier tes sources.");
                else if (credibility < 80)
                    await e.Channel.SendMessage("**" + deserializedProduct.title + "** à l'air d'être un article sérieux.");
                else
                    await e.Channel.SendMessage("**" + deserializedProduct.title + "** est totalement vrai!");
            }
        }

        public static string GetJsonFromURL(string URL)
        {
            WebRequest request = WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            string jsonText;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                jsonText = sr.ReadToEnd();
            }
            return jsonText;
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
