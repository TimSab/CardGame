using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class ConsoleDesk
    {
        public List<CardPairOnDesk> CardPairsOnDesk { get; set; }
        private Game game;
        private int counter = 0;
        //var game = new Game(2);
        //var player = new Player("Жирная свинья");
        //var player1 = new Player("И Бог, и гений");
        //game.TryConnect(player);
        //    game.TryConnect(player1);
        //    game.StartGame();
        //    Console.WriteLine("Игра началась");
        //    Console.WriteLine($"Ходит игрок {game.WhoseTurn.Name}");
        //    foreach (var card in game.WhoseTurn.Hand)
        //    {
        //        Console.WriteLine(card);
        //    }
        public ConsoleDesk(Game game, List<Player> players)
        {
            CardPairsOnDesk = new List<CardPairOnDesk>();
            this.game = game;
            foreach (var player in players)
            {
                game.TryConnect(player);
            }

            game.StartGame();
            Console.WriteLine("Игра началась");
            while (true)
            {
                foreach (var player in game.Players)
                {
                    if (player.Hand.Count <= 0) continue;
                    ShowPlayerCards();
                    MakeMove();
                    СhangeWhoseTurn();
                }
            }

        }

        public void ShowPlayerCards()
        {
            Console.WriteLine($"Козырная масть {game.Deck.TrumpSuit}");
            Console.WriteLine($"Ходит игрок {game.WhoseTurn.Name}");
            Console.WriteLine("Выберите карту, нажав соответствующее число");
            var number = 1;
            foreach (var card in game.WhoseTurn.Hand)
            {
                Console.WriteLine($"{number}) {card}");
                number++;
            }
        }

        public void MakeMove()
        {
            counter++;
            var a = Console.ReadKey().KeyChar;
            var card = new Card();           
            switch (a)
            {
                case '1':
                    card = game.WhoseTurn.Throw(0);
                    break;
                case '2':
                    card = game.WhoseTurn.Throw(1);
                    break;
                case '3':
                    card = game.WhoseTurn.Throw(2);
                    break;
                case '4':
                    card = game.WhoseTurn.Throw(3);
                    break;
                case '5':
                    card = game.WhoseTurn.Throw(4);
                    break;
                case '6':
                    card = game.WhoseTurn.Throw(5);
                    break;
            }

            Console.Clear();
            if(counter % 2 != 0)
            {
                var cardPair = new CardPairOnDesk();
                cardPair.Add(card);
                CardPairsOnDesk.Add(cardPair);
            }
            else if(counter % 2 == 0)
            {
                CardPairsOnDesk.Last().Add(card);
            }
            foreach (var cardPairsOnDesk in CardPairsOnDesk)
            {
                Console.WriteLine(cardPairsOnDesk);
            }
            //cardPair.Add(card);
            //CardPairsOnDesk.Add(cardPair);
            //foreach (var cardPairOnDesk in CardPairsOnDesk)
            //{
            //    Console.WriteLine(cardPairOnDesk);
            //}
            Console.ReadKey();
        }
       
        public void СhangeWhoseTurn()
        {
            foreach (var player in game.Players)
            {
                if (player.ConnectionNumber == (game.WhoseTurn.ConnectionNumber % game.Players.Count) +1)
                {
                    game.WhoseTurn = player;
                    return;
                }
            }
        }
    }
}

