using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class ConsoleDesk
    {
        private const int lastPlayerCount = 2;
        private DurakGame game;

        public ConsoleDesk(DurakGame game)
        {
            this.game = game;
            Console.WriteLine("Игра началась");

            while (true)
            {
                game.Round.StartRound(game);
                CurrentRound(game);
                game.Round.EndRound();
                if (game.Deck.Cards.Count == 0)
                {
                    var playerInGameCount = 0;
                    foreach (var player in game.Players)
                    {
                        if (player.Hand.Count != 0)
                        {
                            playerInGameCount++;
                        }
                    }
                    if (playerInGameCount < lastPlayerCount)
                    {
                        break;
                    }
                }
            }
        }
        public void ShowPlayerCards()
        {
            var number = 1;
            foreach (var card in game.Round.WhoseTurn.Hand)
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
                        Console.WriteLine($"Ходит игрок {game.Round.WhoseTurn.Name}");             //{game.WhoseTurn.Name}
                        Console.WriteLine("Выберите карту, нажав соответствующее число");
                        if (game.Round.WhoseTurn.StatusPlayer != StatusPlayer.defence && game.Round.WhoseTurn.StatusPlayer != StatusPlayer.attack)
                        {
                            Console.WriteLine("0) пропустить ");
                        }
                        if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                        {
                            Console.WriteLine("-1) взять все карты ");
                        }
                        ShowPlayerCards();

                        if (game.Round.WhoseTurn.Hand.Count == 0 && game.Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                        {
                            game.Round.RoundEndResult = RoundEndResult.defended;
                            return;
                        }
                        bool validValue;
                        int input;
                        do
                        {
                            Console.WriteLine("введите число");
                            validValue = int.TryParse(Console.ReadLine(), out input);
                        } while (!validValue);
                        if (input == 0)
                        {
                            if(game.Round.WhoseTurn.StatusPlayer != StatusPlayer.throwsUp)
                            {
                                break;
                            }
                            game.MakeMove(input);
                            game.СhangeWhoseTurn();
                            break;
                        }
                        if (input == -1)
                        {
                            if (game.Round.WhoseTurn.StatusPlayer != StatusPlayer.defence)
                            {

                                game.Round.RoundEndResult = RoundEndResult.notDefended;
                                game.MakeMove(input);
                                game.СhangeWhoseTurn();
                                ShowCardPair();
                                return;
                            }
                            break;
                        }
                        game.MakeMove(input);
                        if (game.Round.WhoseTurn.StatusPlayer != StatusPlayer.defence)
                        {
                            game.Round.ListThrowPlayers = game.Round.GetNewQueueThrowPlayers(game, game.Round.WhoseTurn);
                        }
                        game.СhangeWhoseTurn();
                        Console.Clear();
                        ShowCardPair();
                    }
                }
            }
        }
        public void ShowCardPair()
        {
            foreach (var cardPairsOnDesk in game.CardPairsOnDesk)
            {
                Console.WriteLine(cardPairsOnDesk);
            }
        }
    }
}


