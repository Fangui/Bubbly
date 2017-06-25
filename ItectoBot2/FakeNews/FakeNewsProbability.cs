using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Customsearch.v1;
using unirest_net.http;
using unirest_net.request;

namespace ItectoBot2.FakeNews
{
    static class FakeNewsProbability
    {
        public static Dictionary<string, Tuple<string, float>> Bdd = new Dictionary<string, Tuple<string, float>>()
        {
            {"lemonde", new Tuple<string, float>("left", 95f)},
            {"l'obs", new Tuple<string, float>("left", 95f)},
            {"mediapart", new Tuple<string, float>("left", 95f)},
            {"leparisien.fr", new Tuple<string, float>("centre", 90f)},
            {"bfmtv", new Tuple<string, float>("centre", 95f)},
            {"20minutes.fr", new Tuple<string, float>("right", 95f)},
            {"lefigaro", new Tuple<string, float>("right", 95f)},
            {"facebook.com", new Tuple<string, float>("centre", 95f)},
            {"legorafi", new Tuple<string, float>("centre", 10f)},
            {"nordpresse-toutel'actualité", new Tuple<string, float>("centre", 10f)}
        };
           
        public static List<string> GetLinkedArticles(string name, string k_title)
        {
            List<string> articles = new List<string>();
            const string apiKey = "AIzaSyAbcg5J_EGHnZPsheFYJs4d8zfo29HEbbs";
            const string searchEngineId = "005900688298725835098:1aynrettnsi";
            //string query = k_title;
            var customSearchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(k_title);
            listRequest.Cx = searchEngineId;

            IList<Google.Apis.Customsearch.v1.Data.Result> paging = new List<Google.Apis.Customsearch.v1.Data.Result>();
            var count = 1;

            while (count < 10)
            {
                //Console.WriteLine($"Page {count}");
                listRequest.Start = count;
                paging = listRequest.Execute().Items;
                if (paging != null)
                {
                    foreach (var item in paging)
                    {
                        if (!item.Link.Contains(name))
                        {
                            articles.Add(item.Link);
                            //Console.WriteLine("Title : " + item.Title +
                            //    Environment.NewLine + "Link : " +
                            //    item.Link + Environment.NewLine + Environment.NewLine);
                            count++;
                        }
                    }
                }
                
            }
            //Console.ReadLine();
            return articles;
        }

        //Vérifie la crédibilité des informations
        public static float GetProbability(Dictionary<string, List<string>> infos)
        {
            Program.Log("siteName", LogColor.Message, infos["website"][0]);
            Program.Log("title", LogColor.Message, infos["title"][0]);
            Program.Log("author", LogColor.Message, infos["author"][0]);
            Program.Log("date", LogColor.Message, infos["date"][0]);

            float proba = Bdd.ContainsKey(infos["website"][0]) ? Bdd[infos["website"][0]].Item2 : 50;
            if (infos["author"][0] == "")
                proba -= 16;
            if (infos["date"][0] == "")
                proba -= 16;
            return proba;
        }
    }
}
