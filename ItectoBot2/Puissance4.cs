using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;


namespace ItectoBot2
{
    class Puissance4 : Commande
    {
        static void Call(string msg)
        {

        }



        const string EMPTY_CELL = ":black_large_square:";
        const string CELL_1 = ":red_circle:";
        const string CELL_2 = ":large_blue_circle:";

        User joueur1, joueur2;
        User currentPlayer;
        Channel chan;
        Message lastMessage;
        int[,] grid = new int[7, 6];
        int[] lastFree = new int[7];
        int nbTour = 1;

        Random rand = new Random();
        const int RANDOM_SIMULATION = 100;
        const int ITERATION = 3;
        int couleurIA;
        int couleurAdverse;

        public Puissance4(User joueur1, User joueur2, Channel channel)
        {
            Program.client.MessageReceived += Client_MessageReceived;
            chan = channel;
            this.joueur1 = joueur1;
            this.joueur2 = joueur2;
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    grid[x, y] = 0;
                }
                lastFree[x] = 0;
            }
            Random r = new Random();
            currentPlayer = r.Next(0, 2) == 0 ? joueur1 : joueur2;
            DisplayGrid(CELL_1 + " A " + currentPlayer.Mention + " de jouer.");
            if (currentPlayer.Name == "ItectoBot")
            {
                couleurIA = 1;
                couleurAdverse = 2;
                playIA();
            }
            else
            {
                couleurIA = 2;
                couleurAdverse = 1;
            }
        }

        async private void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            if (e.User.Id == currentPlayer.Id && e.Channel.Id == chan.Id)
            {
                int choix;
                if (int.TryParse(e.Message.Text, out choix) && choix > 0 && choix < 8 && grid[choix - 1, 5] == 0)
                {
                    Program.Log("Puissance4", LogColor.Message, e.Message.ToString());
                    await e.Message.Delete();
                    int x = choix - 1;
                    int y = lastFree[x];
                    lastFree[x]++;
                    grid[x, y] = (currentPlayer == joueur1) ? 1 : 2;
                    if (checkWin(x, y))
                    {
                        if (currentPlayer.Name != "ItectoBot" && (joueur1.Name == "ItectoBot" || joueur2.Name == "ItectoBot"))
                        {
                            //Easter egg
                        }
                        DisplayGrid(currentPlayer.Mention + " a gagné.");
                        Dispose();
                    }
                    else
                    {
                        if (currentPlayer.Id == joueur1.Id)
                            currentPlayer = joueur2;
                        else
                            currentPlayer = joueur1;
                        if (nbTour == 42)
                        {
                            DisplayGrid("Match Nul.");
                            Dispose();
                        }
                        else
                        {
                            DisplayGrid((currentPlayer == joueur1 ? CELL_1 : CELL_2) + " A " + currentPlayer.Mention + " de jouer.");
                            if (currentPlayer.Name == "ItectoBot")
                            {
                                playIA();
                            }
                            nbTour++;
                        }
                    }
                }
            }
        }

        async private void DisplayGrid(string lastLine)
        {
            string textGrid = "|:one:|:two:|:three:|:four:|:five:|:six:|:seven:|\n";
            for (int y = 5; y >= 0; y--)
            {
                textGrid += "|";
                for (int x = 0; x < 7; x++)
                {
                    if (grid[x, y] == 0)
                        textGrid += EMPTY_CELL;
                    else if (grid[x, y] == 1)
                        textGrid += CELL_1;
                    else
                        textGrid += CELL_2;
                    textGrid += "|";
                }
                textGrid += "\n";
            }
            textGrid += " ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨\n";
            Message tmp = await chan.SendMessage(textGrid + lastLine);
            if (lastMessage != null)
                await lastMessage.Delete();
            lastMessage = tmp;
        }

        private bool checkWin(int x, int y)
        {
            int horizontal = 1;
            int vertical = 1;
            int diag1 = 1;
            int diag2 = 1;

            int X = x - 1;
            int Y = y;
            while (X >= 0 && grid[X, Y] == grid[x, y])
            {
                X--;
                horizontal++;
            }
            X = x + 1;
            Y = y;
            while (X < 7 && grid[X, Y] == grid[x, y])
            {
                X++;
                horizontal++;
            }
            if (horizontal >= 4)
                return true;

            X = x;
            Y = y - 1;
            while (Y >= 0 && grid[X, Y] == grid[x, y])
            {
                Y--;
                vertical++;
            }
            if (vertical >= 4)
                return true;

            X = x - 1;
            Y = y - 1;
            while (Y >= 0 && X >= 0 && grid[X, Y] == grid[x, y])
            {
                Y--;
                X--;
                diag1++;
            }
            X = x + 1;
            Y = y + 1;
            while (Y < 6 && X < 7 && grid[X, Y] == grid[x, y])
            {
                Y++;
                X++;
                diag1++;
            }
            if (diag1 >= 4)
                return true;

            X = x - 1;
            Y = y + 1;
            while (Y < 6 && X >= 0 && grid[X, Y] == grid[x, y])
            {
                Y++;
                X--;
                diag2++;
            }
            X = x + 1;
            Y = y - 1;
            while (Y >= 0 && X < 7 && grid[X, Y] == grid[x, y])
            {
                Y--;
                X++;
                diag2++;
            }
            if (diag2 >= 4)
                return true;

            return false;
        }

        private void Dispose()
        {
            Program.client.MessageReceived -= Client_MessageReceived;
            joueur1 = null;
            joueur2 = null;
            currentPlayer = null;
            chan = null;
        }


        async private void playIA()
        {
            await chan.SendIsTyping();
            float max = -1;
            int choosenColumn = 0;
            if (nbTour == 1)
            {
                choosenColumn = 3;
            }
            else
            {
                for (int x = 0; x < 7; x++)
                {
                    if (lastFree[x] != 6)
                    {
                        float result = simulateIA(x, nbTour);
                        if (result > max)
                        {
                            choosenColumn = x;
                            max = result;
                            if (max == 1)
                                break;
                        }
                    }
                }
                Program.Log("Puissance4", LogColor.Message, "Proba win : " + max);
            }
            await chan.SendMessage((choosenColumn + 1) + "");
        }

        private float simulateIA(int choix, int tour)
        {
            grid[choix, lastFree[choix]] = couleurIA;
            lastFree[choix]++;
            float result = 0;

            if (checkWin(choix, lastFree[choix] - 1))
            {
                result = 1;
            }
            else if (tour + 1 == 42)
            {
                result = 0.5f;
            }
            else
            {
                int choosenColumn = 0;
                float min = 2;
                for (int x = 0; x < 7; x++)
                {
                    if (lastFree[x] != 6)
                    {
                        float player = simulatePlayer(x, tour + 1);
                        if (player < min)
                        {
                            choosenColumn = x;
                            min = player;
                            if (min == 0)
                                break;
                        }
                    }
                }
                result = min;
            }

            lastFree[choix]--;
            grid[choix, lastFree[choix]] = 0;
            return result;
        }

        private float simulatePlayer(int choix, int tour)
        {
            grid[choix, lastFree[choix]] = couleurAdverse;
            lastFree[choix]++;
            float result = 0;

            if (checkWin(choix, lastFree[choix] - 1))
            {
                result = 0;
            }
            else if (tour + 1 == 42)
            {
                result = 0.5f;
            }
            else if (tour - nbTour >= ITERATION)
            {
                int choosenColumn = 0;
                float max = -1;
                for (int x = 0; x < 7; x++)
                {
                    if (lastFree[x] != 6)
                    {
                        float IA = think(x);
                        if (IA > max)
                        {
                            choosenColumn = x;
                            max = IA;
                            if (max == 1)
                                break;
                        }
                    }
                }
                result = max;
            }
            else
            {
                int choosenColumn = 0;
                float max = -1;
                for (int x = 0; x < 7; x++)
                {
                    if (lastFree[x] != 6)
                    {
                        float IA = simulateIA(x, tour + 1);
                        if (IA > max)
                        {
                            choosenColumn = x;
                            max = IA;
                            if (max == 1)
                                break;
                        }
                    }
                }
                result = max;
            }

            lastFree[choix]--;
            grid[choix, lastFree[choix]] = 0;
            return result;
        }


        private float think(int choix)
        {
            grid[choix, lastFree[choix]] = couleurIA;
            lastFree[choix]++;
            float sum = RANDOM_SIMULATION;
            if (!checkWin(choix, lastFree[choix] - 1))
            {
                sum = 0;
                for (int i = 0; i < RANDOM_SIMULATION; i++)
                {
                    sum += simulateRandom();
                }
            }
            lastFree[choix]--;
            grid[choix, lastFree[choix]] = 0;
            return sum / RANDOM_SIMULATION;
        }



        private float simulateRandom()
        {
            int nbCoups = 1;
            int[] choix = new int[42];
            bool joueurAdverse = true;
            int delta = ((ITERATION + 1) / 2) * 2;

            int x = rand.Next(0, 7);
            while (lastFree[x] == 6)
            {
                x = (x + 1) % 7;
            }
            int y = lastFree[x];
            lastFree[x]++;
            grid[x, y] = couleurAdverse;
            choix[0] = x;

            while (nbCoups + nbTour + delta != 42 && !checkWin(x, y))
            {
                joueurAdverse = !joueurAdverse;
                x = rand.Next(0, 7);
                while (lastFree[x] == 6)
                {
                    x = (x + 1) % 7;
                }
                y = lastFree[x];
                lastFree[x]++;
                if (joueurAdverse)
                    grid[x, y] = couleurAdverse;
                else
                    grid[x, y] = couleurIA;
                choix[nbCoups] = x;
                nbCoups++;
            }
            for (int i = nbCoups - 1; i >= 0; i--)
            {
                lastFree[choix[i]]--;
                grid[choix[i], lastFree[choix[i]]] = 0;
            }
            if (nbCoups + nbTour + delta == 42)
                return 0.5f;
            return joueurAdverse ? 0 : 1;
        }
    }
}
