using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace ItectoBot2.FakeNews
{
    class Youtube
    {
        
        static string getID(string url)
        {
            int i;
            for (i = 0; i < url.Length - 1; i++)
            {
                if (url[i] == 'v' && url[i + 1] == '=')
                {
                    break;
                }
            }
            i += 2;
            string ret = "";
            while (url.Length > i && url[i] != '&')
            {
                ret += url[i];
                ++i;
            }
            return ret;
        }
        public static string HelloYoutube(string url)
        {
            string id = getID(url);
            if (id == "")
                return "";
            string URL = "https://www.googleapis.com/youtube/v3/videos?id=" + id + "&key=" + "AIzaSyAYGftLRE17bOaNLM0oHeXWhd-OUeW1S8o" + "&part=statistics,snippet";
            string jsonText = FakeNewsManager.GetJsonFromURL(URL);
            JObject jo = JObject.Parse(jsonText);
            int dislike = int.Parse(jo.SelectToken("items[0].statistics.dislikeCount").ToString());
            int like = int.Parse(jo.SelectToken("items[0].statistics.likeCount").ToString());
            float ratiolike = (float)like / (float)(like + dislike)*100;
            string title = jo.SelectToken("items[0].snippet.localized.title").ToString();
            bool IALLCAPS = title.ToUpper() == title;
            URL = "https://www.googleapis.com/youtube/v3/commentThreads?videoId=" + id + "&key=AIzaSyAYGftLRE17bOaNLM0oHeXWhd-OUeW1S8o" + "&part=snippet";
            jsonText = FakeNewsManager.GetJsonFromURL(URL);
            jo = JObject.Parse(jsonText);
            int numOfComplain = 0;
            for (int i = 0; i < 20 || i < int.Parse(jo.SelectToken("pageInfo.totalResults").ToString()); ++i) {
                string s = jo.SelectToken("items["+i.ToString()+"].snippet.topLevelComment.snippet.textDisplay").ToString();
                s = s.ToLower();
                if (s.Contains("clickbait")||s.Contains("putaclick")||s.Contains("fake title")|| s.Contains("putaclic")||s.Contains("Putaclique"))
                {
                    numOfComplain++;
                }
            }

            float proba = ratiolike;
            proba -= 10 * numOfComplain;
            if (IALLCAPS)
            {
                proba -= 20;
                if (proba > 50)
                {
                    Program.Log("note vidéo", LogColor.Message, proba.ToString());
                    return "Vidéo cool, mais pas sérieuse";
                }
            }
            Program.Log("note vidéo", LogColor.Message, proba.ToString());
            return proba > 50 ? "Vidéo sérieuse." : "Vidéo pas sérieuse.";
        }
    }
}
