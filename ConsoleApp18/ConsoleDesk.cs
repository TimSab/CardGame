using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class ConsoleDesk
    {
        private DurakGame game;
        private RoundEndResult? resultLastRound = null;

        public ConsoleDesk(DurakGame game)
        {
            this.game = game;
            Console.WriteLine("Игра началась");

            while (true)
            {
                game.Round = new Round();
                game.Round.StartRound(resultLastRound, game);
                CurrentRound(game);
                game.Round.EndRound(resultLastRound);
                if (game.Deck.Cards.Count == 0)
                {
                    var counter = 0;
                    foreach (var player in game.Players)
                    {
                        if (player.Hand.Count != 0)
                        {
                            counter++;
                        }
                    }
                    if (counter < 2)
                    {
                        break;
                    }
                }
            }
        }
        public void ShowPlayerCards()
        {
            var number = 1;
            foreach (var card in game.WhoseTurn.Hand)
            {
                Console.WriteLine($"{number}) {card}");
                number++;
            }
        }

        public void CurrentRound(DurakGame game)
        {
            while (true)
            {
                for (int i = 0; i < game.Players.Count; i++)
                {
                    while (true) // этот цикл нужен чтобы заствавить игрока если он не подкидывает и нажал кнопку  пропустить - ходить еще раз.
                    {
                        Console.WriteLine($"Козырная масть {game.Deck.TrumpSuit}");
                        Console.WriteLine($"Ходит игрок {game.WhoseTurn.Name}");             //{game.WhoseTurn.Name}
                        Console.WriteLine("Выберите карту, нажав соответствующее число");
                        Console.WriteLine("0) пропустить ");
                        ShowPlayerCards();
                        Console.WriteLine("q) взять все карты ");
                        if (game.WhoseTurn.Hand.Count == 0 && game.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                        {
                            game.Round.RoundEndResult = RoundEndResult.defended;
                            resultLastRound = RoundEndResult.defended;
                            return;
                        }
                        var output1 = Console.ReadKey().KeyChar;
                        Console.Write("\b \b");
                        var output2 = Console.ReadKey().KeyChar;
                        Console.Write("\b \b");
                        var flag2 = true;                        
                        if (output1 == 'q')
                        {
                            game.Round.RoundEndResult = RoundEndResult.notDefended;
                            resultLastRound = RoundEndResult.notDefended;
                            game.MakeMove(output1, output2, ref flag2);
                            return;
                        }
                        game.MakeMove(output1, output2, ref flag2);
                        if (flag2 == true)
                        {
                            break;
                        }
                        else
                        {
                            Console.Clear();
                        }
                    }
                }

            }
        }
    }
}

