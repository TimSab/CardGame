using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class Game
    {
        public const int MaxPlayers = 6;
        public const int LimiterHandOutCards = 6;
        public Deck Deck { get; private set; }
        public Player WhoseTurn { get; set; }
        public List<Player> Players { get; set; }

        public Game(int countPlayers)
        {
            if (countPlayers > MaxPlayers) throw new Exception("слишком много игроков");
            if (countPlayers <= 1) throw new Exception("слишком мало игроков");

            Players = new List<Player>(countPlayers);
        }

        public bool TryConnect(Player player)
        {
            try
            {
                Players.Add(player);
            }
            catch
            {
                return false;
            }
            player.ConnectionNumber = Players.Count;
            return true;
        }
        public void StartGame()
        {
            Deck = new Deck();
            Deck.Shuffle();
            CardComparer.TrumpSuit = Deck.TrumpSuit;
            HandOutCards(Players);
            WhoseTurn = DecideWhoseTurn(Players);
        }

        public void StopGame()
        {
            Deck = null; ;
            Players = null;
        }

        public Player DecideWhoseTurn(List<Player> players)
        {
            Player firstPlayer = null;
            Card minTrumpCard = new Card(Deck.TrumpSuit, CardValue.Ace);
            //масть не имеет значение.
            Card maxNotTrump = new Card(CardSuit.Club, CardValue.Six);

            foreach (var player in players)
            {
                var GetMaxNotTrumpOrMinTrumpPlayer = GetMaxNotTrumpOrMinTrump(player);
                if (GetMaxNotTrumpOrMinTrumpPlayer.Suit == Deck.TrumpSuit)
                {
                    if (GetMaxNotTrumpOrMinTrumpPlayer <= minTrumpCard)
                    {
                        minTrumpCard = GetMaxNotTrumpOrMinTrumpPlayer;
                        firstPlayer = player;
                    }
                }
                else
                {
                    if (GetMaxNotTrumpOrMinTrumpPlayer.Value >= maxNotTrump.Value)
                    {
                        maxNotTrump = GetMaxNotTrumpOrMinTrumpPlayer;
                        firstPlayer = player;
                    }
                }
            }

            return firstPlayer;
            
            
            //foreach (var player in players)
            //{
            //    var minTrumpPlayer = GetMinTrump(player);
            //    if (minTrumpPlayer <= minTrumpCard)
            //    {
            //        minTrumpCard = minTrumpPlayer;
            //        firstPlayer = player;
            //    }
            //}

            //if (firstPlayer == null)
            //{
            //    foreach (var player in players)
            //    {
            //        if (GetMaxNotTrump(player).Value >= maxNotTrump.Value)
            //        {
            //            maxNotTrump = GetMaxNotTrump(player);
            //            firstPlayer = player;
            //        }
            //    }
            //}

            //if (firstPlayer != null) return firstPlayer;
            //foreach (var player in players)
            //{
            //    if (player.ConnectionNumber == 1) return player;
            //}
            ////return null;
            //throw new Exception("невозможно определить первый ход");
        }

        private void HandOutCards(List<Player> players)
        {
            foreach (var player in players)
            {
                while (player.Hand.Count < LimiterHandOutCards)
                {
                    if (Deck.Cards.Count <= 0) return;
                    player.Hand.Add(Deck.Cards.Pop());
                }
            }
        }


        public Card GetMinTrump(Player player)
        {
            if (player.Hand.Count == 0) return null;
            var min = new Card();
            bool found = false;

            foreach (var card in player.Hand)
            {
                if (card.Suit == Deck.TrumpSuit)
                {
                    min = card;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                foreach (var card in player.Hand)
                {
                    if (card.Suit == Deck.TrumpSuit)
                        if (card.Value < min.Value) min = card;
                }
            }
            else return null;

            return min;
        }

        public Card GetMaxNotTrump(Player player)
        {
            if (player.Hand.Count == 0) return null;
            var max = new Card();
            bool found = false;

            foreach (var card in player.Hand)
            {
                if (card.Suit != Deck.TrumpSuit)
                {
                    max = card;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                foreach (var card in player.Hand)
                {
                    if (card.Suit != Deck.TrumpSuit)
                        if ((int)card.Value > (int)max.Value) max = card;
                }
            }
            else return null;

            return max;
        }

        public Card GetMaxNotTrumpOrMinTrump(Player player)
        {
            Card minTrumpCard = new Card(Deck.TrumpSuit, CardValue.Ace);
            //масть не имеет значение.
            Card maxNotTrumpCard = new Card(CardSuit.Club, CardValue.Six);

            var minTrumpPlayer = GetMinTrump(player);
            if (minTrumpPlayer is null)
            {
                var maxNotTrumpPlayer = GetMaxNotTrump(player);
                if (maxNotTrumpPlayer.Value >= maxNotTrumpCard.Value)
                {
                    maxNotTrumpCard = maxNotTrumpPlayer;
                    return maxNotTrumpCard;
                }
            }
            else
            {
                if (minTrumpPlayer <= minTrumpCard)
                {
                    minTrumpCard = minTrumpPlayer;
                    return minTrumpCard;
                }
            }
            return null;
        }
    }
}

enum GameAction
{
    Throw,
    Pass,
    GetAll
}

enum GameResult
{
    Win,
    Loose,
    Draw
}

