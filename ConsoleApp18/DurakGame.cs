using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ДОПИСАТЬ ОПРЕДЕЛЕНИЕ ЛАСТ РАУНДА(ТО ЕСТЬ МЕТОД ВЫЯСНЯЮЩИЙ КАК ЗАКОНЧИЛСЯ ПОСЛЕДНИЙ РАУН И ДАВАТЬ ЭТОТ СТАТУС СВОЙСТВУ LastRoundStatus)
namespace ConsoleApp18
{
    public class DurakGame : Game
    {
        public const int MaxPlayers = 6;
        public const int LimiterHandOutCards = 6;
        public override Deck Deck { get; set; }        
        public override List<Player> Players { get; set; }
        public List<CardPairOnDesk> CardPairsOnDesk { get; set; }
        public Round Round { get; set; } // надо постоянно менять(каждый раунд).

        public DurakGame(int countPlayers)
        {
            if (countPlayers > MaxPlayers) throw new Exception("слишком много игроков");
            if (countPlayers <= 1) throw new Exception("слишком мало игроков");

            Players = new List<Player>(countPlayers);
        }

        public override void StartGame()
        {
            Deck = new Deck(36);
            //Deck.Shuffle();
            CardComparer.TrumpSuit = Deck.TrumpSuit;
            Round = new Round();
            HandOutCards();
            
            CardPairsOnDesk = new List<CardPairOnDesk>();
        }

        public override void StopGame()
        {
            Deck = null;
            Players = null;
        }

        public override void HandOutCards()
        {
            foreach (var player in Players)
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
            Card min = null;
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
            Card max = null;
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

        public void MakeMove(int output) // принимает аргумент, чтобы потом он мог браться как с консоли так и с формы.
        {
            if (output > Round.WhoseTurn.Hand.Count || output < -1) return;
            Card card = null;
            if (output > 0)
            {
                card = Round.WhoseTurn.Throw(output - 1);
            }
            if(output == -1)
            {
                Round.WhoseTurn.GetAll(CardPairsOnDesk);
            }
            if (Round.WhoseTurn.StatusPlayer == StatusPlayer.attack)
            {
                Round.WhoseTurn.StatusPlayer = StatusPlayer.throwsUp;
            }
            if (!(card is null))
            {
                if (Round.WhoseTurn.StatusPlayer == StatusPlayer.throwsUp)
                {
                    var cardPair = new CardPairOnDesk();
                    cardPair.Add(card);
                    CardPairsOnDesk.Add(cardPair);
                }
                else if (Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
                {
                    CardPairsOnDesk.Last().Add(card);
                }
            }
        }

        public override void СhangeWhoseTurn() //создать стэк подкидывающих игроков, после хода атакующи игрок тоже добавляется в этот стэк.
        {
            if(Round.WhoseTurn.StatusPlayer == StatusPlayer.defence)
            {
                Round.WhoseTurn = Round.ListThrowPlayers.First();

            }
            else
            {
                Round.WhoseTurn = Round.DefencePlayer;
            }
        }

        public void SetStatusPlayer()
        {
            if (Round.LastRoundResult is null) //такое возможно если это первый раунд в игре.
            {
                Player attackPlayer = null;
                foreach (var player in Players)
                {
                    if (player.StatusPlayer == StatusPlayer.attack)
                    {
                        attackPlayer = player;
                    }
                }
                foreach (var player in Players)
                {
                    if (player.ConnectionNumber == attackPlayer.ConnectionNumber % Players.Count + 1)
                    {
                        player.StatusPlayer = StatusPlayer.defence;
                    }
                    else if (player.StatusPlayer != StatusPlayer.attack)
                    {
                        player.StatusPlayer = StatusPlayer.throwsUp;
                    }
                }
            }
            else if (Round.LastRoundResult == RoundEndResult.defended)
            {
                Player attackPlayer = null;
                foreach (var player in Players)
                {
                    if (player.StatusPlayer == StatusPlayer.defence)
                    {
                        player.StatusPlayer = StatusPlayer.attack;
                        attackPlayer = player;
                    }
                }
                foreach (var player in Players)
                {
                    if (player.ConnectionNumber == attackPlayer.ConnectionNumber % Players.Count + 1)
                    {
                        player.StatusPlayer = StatusPlayer.defence;
                    }
                    else if (player.StatusPlayer != StatusPlayer.attack)
                    {
                        player.StatusPlayer = StatusPlayer.throwsUp;
                    }
                }
            }
            //else if (Round.RoundStatus == RoundStatus.continues)
            //{
            //    return;
            //}
            else if (Round.LastRoundResult == RoundEndResult.notDefended)
            {
                Player pastDefencePlayer = null;
                foreach (var player in Players)
                {
                    if (player.StatusPlayer == StatusPlayer.defence)
                    {
                        pastDefencePlayer = player;
                    }
                }

                Player attackPlayer = null;
                foreach (var player in Players)
                {
                    if (player.ConnectionNumber == pastDefencePlayer.ConnectionNumber % Players.Count + 1)
                    {
                        player.StatusPlayer = StatusPlayer.attack;
                        attackPlayer = player;
                    }
                }
                foreach (var player in Players)
                {
                    if (player.ConnectionNumber == attackPlayer.ConnectionNumber % Players.Count + 1)
                    {
                        player.StatusPlayer = StatusPlayer.defence;
                    }
                    else if (player.StatusPlayer != StatusPlayer.attack)
                    {
                        player.StatusPlayer = StatusPlayer.throwsUp;
                    }
                }
            }
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


