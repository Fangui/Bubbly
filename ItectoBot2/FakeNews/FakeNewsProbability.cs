using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItectoBot2.FakeNews
{
    static class FakeNewsProbability
    {
        static Dictionary<string, float> dataBase;
        
        //Vérifie la crédibilité des informations
        public static float GetProbability(Dictionary<string, List<string>> infos)
        {
            float proba = dataBase.ContainsKey(infos["website"][0])? dataBase[infos["website"][0]] : 30; //Index 0 should contains the key
    
            return proba;
        }
    }
}
