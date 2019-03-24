using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    class Program
    {
        //private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        //{
        //    Console.WriteLine((args.ExceptionObject as Exception).Message);
        //    Environment.Exit(1);
        //}

        static void Main(string[] args)
        {

            //    AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;
            var countPlayers = 6;
            var game = new DurakGame(countPlayers);
            var players = new List<Player>(countPlayers);
            for (int i = 1; i <= countPlayers; i++)
            {
                players.Add(new Player(i.ToString()));
            }

            foreach (var player in players)
            {
                game.TryConnect(player);
            }
            game.StartGame();
            var desk = new ConsoleDesk(game);
        }
    }
}
