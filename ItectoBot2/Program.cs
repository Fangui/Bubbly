using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace ItectoBot2
{
    class Program
    {

        static public DiscordClient client;
        static string HELP =
@"```html
Commandes ItectoBot:

    !puissance4 <User>
    !<Text>
```
";


        public static void Main()
        {
            client = new DiscordClient(input =>
            {
                input.LogLevel = LogSeverity.Info;
            });
            client.MessageReceived += Client_MessageReceived;
            client.ExecuteAndWait(async () =>
            {
                await client.Connect("MjA1Mzk4OTMyNjM1Nzc5MDcz.DAxzoA.eNxFmlCFRu8wmLirfdzw_loeOYc", TokenType.Bot);
                Log("Infos", LogColor.Info, "Connection succeeds");
                while(true)
                {
                    Console.ReadKey(true);
                    Log("Ping", LogColor.Info, "");
                }
            });
        }

        static async private void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            if (!e.User.IsBot && e.Message.Text.Length > 0 && e.Message.Text[0] == '!')
            {
                string[] messages = e.Message.Text.Split(' ');
                switch (messages[0])
                {
                    case "!help":
                        await e.Channel.SendMessage(HELP);
                        break;
                    case "!puissance4":
                        if (e.Message.MentionedUsers.Count() > 0)
                        {
                            new Puissance4(e.User, e.Message.MentionedUsers.First(), e.Channel);
                            await e.Message.Delete();
                        }
                        else
                        {
                            await e.Channel.SendMessage("```\nChoisis un adversaire.\nExemple: !puissance4 @ItectoBot#6877\n```");
                        }
                        break;
                    default:
                        await e.Channel.SendMessage("```cSharp\n//" + e.User.Name + " :\n" + e.Message.Text.Substring(1, e.Message.Text.Length - 1) + "\n```");
                        await e.Message.Delete();
                        break;
                }
                Log(e.Channel.Name, LogColor.Commande, e.Message.ToString());
            }
        }

        public static void Log(string titre, LogColor color, string msg)
        {

            switch (color)
            {
                case LogColor.Info:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogColor.Commande:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogColor.Message:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
            string balise = "<" + titre + "|" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ">";
            string espace = "";
            if (balise.Length < 25)
                espace = new string(' ', 25 - balise.Length);
            Console.WriteLine(balise + espace + msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    enum LogColor
    {
        Info,
        Commande,
        Message
    }
}
