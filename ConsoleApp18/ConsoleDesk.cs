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
                if (game.Deck.Cards.Count == 0) // проверка условий для окончания игры.
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
            for (int i = 1; i < game.Players.Count-1; i++)
            {
                while (true) // этот цикл нужен чтобы заствавить игрока если он не подкидывает и нажал кнопку  пропустить - ходить еще раз.
                {
                    Console.WriteLine($"Козырная масть {game.Deck.TrumpSuit}");
                    Console.WriteLine($"Ходит игрок {game.Round.WhoseTurn.Name}");             //{game.WhoseTurn.Name}
                    Console.WriteLine("Выберите карту, нажав соответствующее число");
                    if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.throwsUp)
                    {
                        Console.WriteLine("0) пропустить ");
                    }
                    if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                    {
                        Console.WriteLine("-1) взять все карты ");
                    }
                    ShowPlayerCards();

                    if (game.Round.DefencePlayer.Hand.Count == 0)
                    {
                        game.Round.RoundEndResult = RoundEndResult.defended;
                        Console.Clear();
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
                        if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.throwsUp)
                        {
                            game.MakeMove(input);
                            game.Round.WhoseTurn = game.Round.ListThrowPlayers[i];
                            //game.Round.ListThrowPlayers.RemoveAt(i);
                            //game.СhangeWhoseTurn();
                            Console.Clear();
                            ShowCardPair();
                            break;
                        }
                    }
                    if (input == -1)
                    {
                        if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                        {

                            game.Round.RoundEndResult = RoundEndResult.notDefended;
                            game.MakeMove(input);
                            game.СhangeWhoseTurn();                            
                            Console.Clear();
                            ShowCardPair();
                            return;
                        }
                        //Console.Clear();
                        //break;
                    }
                    if (input > 0 && input <= game.Round.WhoseTurn.Hand.Count)
                    {
                        game.MakeMove(input);
                        if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                        {                                                        
                            i = 1; // если игрок отбился значит  появилась новая карта на столе которая возможно у кого то есть, поэтому 
                            // обновляем фор и опять проходим по всем игрокам пока они все не пропустят ход.
                        }
                        if (game.Round.WhoseTurn.StatusPlayer == StatusPlayer.throwsUp)
                        {
                            game.Round.ListThrowPlayers = game.Round.GetNewQueueThrowPlayers(game, game.Round.WhoseTurn);
                        }
                        game.СhangeWhoseTurn();
                        //Console.Clear();
                        //ShowCardPair();
                    }
                    Console.Clear();
                    ShowCardPair();
                }
            }
            game.Round.RoundEndResult = RoundEndResult.defended;
            return;
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


