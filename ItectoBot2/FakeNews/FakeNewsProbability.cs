using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Customsearch.v1;

namespace ItectoBot2.FakeNews
{
    static class FakeNewsProbability
    {
        public static Dictionary<string, Tuple<string, float>> Bdd()
        {
            Dictionary<string, Tuple<string, float>> bdd = new Dictionary<string, Tuple<string, float>>();
            bdd.Add("nouvel obs", new Tuple<string, float>("left", 0.95f));
            bdd.Add("mediapart", new Tuple<string, float>("left", 0.95f));
            bdd.Add("le parisien", new Tuple<string, float>("centre", 0.90f));
            bdd.Add("bfm tv", new Tuple<string, float>("centre", 0.95f));
            bdd.Add("20minutes", new Tuple<string, float>("right", 0.95f));
            bdd.Add("le figaro", new Tuple<string, float>("right", 0.95f));
            bdd.Add("le gorafi", new Tuple<string, float>("centre", 0.1f));
            bdd.Add("nordpresse", new Tuple<string, float>("centre", 0.1f));
            return bdd;
        }
        static Dictionary<string, float> dataBase;



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
            float proba = dataBase.ContainsKey(infos["website"][0])? dataBase[infos["website"][0]] : 50; //Index 0 should contains the key
    
            return proba;
        }

        public static float ProbabilityIsAll(float pertinence)
        {
            return pertinence;
        }
    }
}
