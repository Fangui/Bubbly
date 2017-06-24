using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;

namespace ItectoBot2
{
    class REP 
    {
        public string pageUrl;
        public string resolvedPageUrl;
        public REP(string pageUrl, string resolvedPageUrl)
        {
            {
                this.pageUrl = pageUrl;
                this.resolvedPageUrl = resolvedPageUrl;
            }    
        }
    }

    static class FakeNewsExtraction
    {
        private static REP deserializedUser;

        //Extrait les mots clés de l'article
        public static Dictionary<string, List<string>> Extract(string link)
        {
            string test = "https://api.diffbot.com/v3/analyze?token=6e121650bbc3a88369784060b046e1bf&url=" + link;
            Console.WriteLine(test);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(test);//
            request.Method = "Get";
            request.KeepAlive = true;
            request.ContentType = "appication/json";
            //request.Headers.Add("Content-Type", "appication/json");
            //request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                //myResponse = sr.ReadToEnd();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sr.ReadToEnd()));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
                deserializedUser = ser.ReadObject(ms) as REP;
                ms.Close();
            }
                Console.WriteLine(deserializedUser.pageUrl);
                Console.WriteLine(deserializedUser.resolvedPageUrl);
            // Console.WriteLine(myResponse);
            //response.Write(myResponse);
            Dictionary<string,List<string>> result= new Dictionary<string, List<string>>();

            /*result.Add("Title", new List<string>());
            result["Title"].Add("")*/

           
            return result;
        }
    }
}
