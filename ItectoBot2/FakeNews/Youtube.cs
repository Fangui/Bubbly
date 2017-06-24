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
        public static void HelloYoutube(string url)
        {

            string id = getID(url);
            Console.WriteLine(id);
            if (id == "")
                return;
            string URL = "https://www.googleapis.com/youtube/v3/videos?id=" + id + "&key=" + "AIzaSyAYGftLRE17bOaNLM0oHeXWhd-OUeW1S8o" + "&part=statistics";
            string jsonText=  FakeNewsManager.GetJsonFromURL(URL);
            JObject jo = JObject.Parse(jsonText);
            int dislike = int.Parse(jo.SelectToken("items[0].statistics.dislikeCount").ToString());
            int like = int.Parse(jo.SelectToken("items[0].statistics.likeCount").ToString());
            float ratiolike = like / (like + dislike);
            Console.WriteLine(dislike);
            Console.WriteLine(like);
            URL = "https://www.googleapis.com/youtube/v3/commentThreads?videoId=" + id + "&key=AIzaSyAYGftLRE17bOaNLM0oHeXWhd-OUeW1S8o" + "&part=snippet";
            jsonText = FakeNewsManager.GetJsonFromURL(URL);
            jo = JObject.Parse(jsonText);
            string s = jo.SelectToken("items[0].snippet.topLevelComment.textDisplay").ToString();
            Console.WriteLine(s);

        }
    }
}
