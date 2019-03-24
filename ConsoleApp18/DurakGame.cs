using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ДОПИСАТЬ ОПРЕДЕЛЕНИЕ ЛАСТ РАУНДА(ТО ЕСТЬ МЕТОД ВЫЯСНЯЮЩИЙ КАК ЗАКОНЧИЛСЯ ПОСЛЕДНИЙ РАУН И ДАВАТЬ ЭТОТ СТАТУС СВОЙСТВУ LastRoundStatus)
namespace ConsoleApp18
{
    public class DurakGame
    {
        public const int MaxPlayers = 6;
        public const int LimiterHandOutCards = 6;
        public Deck Deck { get; private set; }
        public Player WhoseTurn { get; set; }
        public List<Player> Players { get; set; }
        public List<CardPairOnDesk> CardPairsOnDesk { get; set; }
        public Round Round { get; set; } // надо постоянно менять(каждый раунд).

        public DurakGame(int countPlayers)
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
            Deck = new Deck(36);
            //Deck.Shuffle();
            CardComparer.TrumpSuit = Deck.TrumpSuit;
            Round = new Round();
            HandOutCards(Players);
            WhoseTurn = DecideWhoseTurn(Players);
            CardPairsOnDesk = new List<CardPairOnDesk>();
        }

        public void StopGame()
        {
            Deck = null;
            Players = null;
        }

        public Player DecideWhoseTurn(List<Player> players)
        {
            Player firstPlayer = null;
            var fufel = true;
            Card minTrumpCard = new Card(Deck.TrumpSuit, CardValue.Ace);
            //масть не имеет значение.
            Card maxNotTrump = new Card(CardSuit.Club, CardValue.Six);

            foreach (var player in players)
            {
                var GetMaxNotTrumpOrMinTrumpPlayer = GetMaxNotTrumpOrMinTrump(player);
                if (GetMaxNotTrumpOrMinTrumpPlayer.Suit == Deck.TrumpSuit)      // Окей, две ОДИНАКОВЫХ КОЗЫРКИ у разных игроков
                {                                                               // не может быть!
                    if (GetMaxNotTrumpOrMinTrumpPlayer <= minTrumpCard)
                    {
                        minTrumpCard = GetMaxNotTrumpOrMinTrumpPlayer;
                        firstPlayer = player;
                        fufel = false;
                    }
                }
                else if (fufel)
                {
                    if (GetMaxNotTrumpOrMinTrumpPlayer.Value >= maxNotTrump.Value)  // Но здесь, два ОДИНАКОВЫХ ЗНАЧЕНИЯ у разных игроков
                    {                                                               // могут быть! И ты отдаешь предпочтение последнему?
                        maxNotTrump = GetMaxNotTrumpOrMinTrumpPlayer;
                        firstPlayer = player;                                       // А также ты отдаешь предпочтение НЕ козырки,
                    }                                                               // ПОСЛЕ козырки.
                }
            }

            //firstPlayer.StatusPlayer = StatusPlayer.attack;
            //SetStatusPlayer();
            firstPlayer.StatusPlayer = StatusPlayer.attack;
            return firstPlayer;
        }

        public void HandOutCards(List<Player> players)
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

        private int counter = 0;
        //bool flag1 = true;
        public void MakeMove(char output1, char output2, ref bool flag2) // принимает аргумент, чтобы потом он мог браться как с консоли так и с формы.
        {
            counter++;
            var flag = true;
            Card card = null;
            while (flag)
            {
                switch (output1)
                {
                    case 'q':
                        if (WhoseTurn.StatusPlayer == StatusPlayer.defence)
                        {
                            WhoseTurn.GetAll(CardPairsOnDesk);
                            flag = false;
                            break;
                        }
                        else
                        {
                            counter--;
                            flag = false;
                            break;
                        }
                    case '0':
                        switch (output2)
                        {
                            case '0':
                                if (WhoseTurn.StatusPlayer == StatusPlayer.throwsUp)
                                {
                                    counter--;
                                    flag = false;
                                    break;
                                }
                                else
                                {
                                    counter--;
                                    flag2 = false;
                                    return;
                                }
                            case '1':
                                card = WhoseTurn.Throw(0);
                                flag = false;
                                break;
                            case '2':
                                card = WhoseTurn.Throw(1);
                                flag = false;
                                break;
                            case '3':
                                card = WhoseTurn.Throw(2);
                                flag = false;
                                break;
                            case '4':
                                card = WhoseTurn.Throw(3);
                                flag = false;
                                break;
                            case '5':
                                card = WhoseTurn.Throw(4);
                                flag = false;
                                break;
                            case '6':
                                card = WhoseTurn.Throw(5);
                                flag = false;
                                break;
                            case '7':
                                card = WhoseTurn.Throw(6);
                                flag = false;
                                break;
                            case '8':
                                card = WhoseTurn.Throw(7);
                                flag = false;
                                break;
                            case '9':
                                card = WhoseTurn.Throw(8);
                                flag = false;
                                break;
                            default:
                                counter--;
                                break;
                        }
                        break;
                    case '1':
                        switch (output2)
                        {
                            case '0':
                                card = WhoseTurn.Throw(9);
                                flag = false;
                                break;
                            case '1':
                                card = WhoseTurn.Throw(10);
                                flag = false;
                                break;
                            case '2':
                                card = WhoseTurn.Throw(11);
                                flag = false;
                                break;
                            case '3':
                                card = WhoseTurn.Throw(12);
                                flag = false;
                                break;
                            case '4':
                                card = WhoseTurn.Throw(13);
                                flag = false;
                                break;
                            case '5':
                                card = WhoseTurn.Throw(14);
                                flag = false;
                                break;
                            case '6':
                                card = WhoseTurn.Throw(15);
                                flag = false;
                                break;
                            case '7':
                                card = WhoseTurn.Throw(16);
                                flag = false;
                                break;
                            case '8':
                                card = WhoseTurn.Throw(17);
                                flag = false;
                                break;
                            case '9':
                                card = WhoseTurn.Throw(18);
                                flag = false;
                                break;
                            default:
                                counter--;
                                break;
                        }
                        break;
                    case '2':
                        switch (output2)
                        {
                            case '0':
                                card = WhoseTurn.Throw(19);
                                flag = false;
                                break;
                            case '1':
                                card = WhoseTurn.Throw(20);
                                flag = false;
                                break;
                            case '2':
                                card = WhoseTurn.Throw(21);
                                flag = false;
                                break;
                            case '3':
                                card = WhoseTurn.Throw(22);
                                flag = false;
                                break;
                            case '4':
                                card = WhoseTurn.Throw(23);
                                flag = false;
                                break;
                            case '5':
                                card = WhoseTurn.Throw(24);
                                flag = false;
                                break;
                            case '6':
                                card = WhoseTurn.Throw(25);
                                flag = false;
                                break;
                            case '7':
                                card = WhoseTurn.Throw(26);
                                flag = false;
                                break;
                            case '8':
                                card = WhoseTurn.Throw(27);
                                flag = false;
                                break;
                            case '9':
                                card = WhoseTurn.Throw(28);
                                flag = false;
                                break;
                            default:
                                counter--;
                                break;
                        }
                        break;
                    case '3':
                        switch (output2)
                        {
                            case '0':
                                card = WhoseTurn.Throw(29);
                                flag = false;
                                break;
                            case '1':
                                card = WhoseTurn.Throw(30);
                                flag = false;
                                break;
                            case '2':
                                card = WhoseTurn.Throw(31);
                                flag = false;
                                break;
                            case '3':
                                card = WhoseTurn.Throw(32);
                                flag = false;
                                break;
                            case '4':
                                card = WhoseTurn.Throw(33);
                                flag = false;
                                break;
                            case '5':
                                card = WhoseTurn.Throw(34);
                                flag = false;
                                break;
                            case '6':
                                card = WhoseTurn.Throw(35);
                                flag = false;
                                break;
                            default:
                                counter--;
                                break;
                        }
                        break;
                }
            }
            Console.Clear();
            if (!(card is null))
            {
                if (counter % 2 != 0)
                {
                    var cardPair = new CardPairOnDesk();
                    cardPair.Add(card);
                    CardPairsOnDesk.Add(cardPair);
                }
                else if (counter % 2 == 0)
                {
                    CardPairsOnDesk.Last().Add(card);
                }
                foreach (var cardPairsOnDesk in CardPairsOnDesk)
                {
                    Console.WriteLine(cardPairsOnDesk);
                }
            }
            if (WhoseTurn.StatusPlayer == StatusPlayer.attack)
            {
                WhoseTurn.StatusPlayer = StatusPlayer.throwsUp;
            }
            СhangeWhoseTurn();
            
        }
        #region

        private Player throwsUpPlayer = null;
        public void GetFirstThrowsUpPlayer() //ВЫЗЫВАТЬ В НАЧАЛЕ РАУНДА ЧТОБЫ ОПРЕДЕЛИТЬ ПЕРВОГО ПОДКИДЫВАЮЩЕГО ИГРОКА.
        {
            foreach (var player in Players)
            {
                if (player.ConnectionNumber == Players.Where(p => p.StatusPlayer == StatusPlayer.defence).First().ConnectionNumber % Players.Count + 1)
                {
                    throwsUpPlayer = player;
                }
            }
        }
        private int counterAttacks = 2; // потомучто карту кинул уже атакующий игрок и идет 2 ход.
        private void GetDefenceCurrentRound()
        {
        }
        private void СhangeWhoseTurn()
        {
            //если произошла бита, то ходит игрок со статусом отбивающий(его статус меняется на ходящий)
            //если отбивающий игрок взял карты, то ходит игрок с конекшн намбером на 1 польше чем у отбивающего(его статус меняется на подкидывающий)

            foreach (var player in Players)
            {
                if (counterAttacks % 2 != 0)
                {
                    if (player.ConnectionNumber == throwsUpPlayer.ConnectionNumber
                        && player.StatusPlayer == StatusPlayer.throwsUp)
                    {
                        WhoseTurn = player;
                        foreach (var player1 in Players)
                        {
                            if (player1.ConnectionNumber == throwsUpPlayer.ConnectionNumber % Players.Count + 1
                                && player1.StatusPlayer == StatusPlayer.throwsUp)
                            {
                                throwsUpPlayer = player1;
                                break;
                            }
                            else 
                            if (player1.ConnectionNumber == throwsUpPlayer.ConnectionNumber % Players.Count + 1
                                && player1.StatusPlayer == StatusPlayer.defence)
                            {
                                foreach (var player2 in Players)
                                {
                                    if (player2.ConnectionNumber == throwsUpPlayer.ConnectionNumber % Players.Count + 1)
                                    {
                                        throwsUpPlayer = player2;
                                        break;
                                    }
                                }
                            }
                        }
                        counterAttacks++;
                        return;
                    }
                }
                else if (counterAttacks % 2 == 0)
                {
                    foreach (var player_ in Players)
                    {
                        if (player_.StatusPlayer == StatusPlayer.defence)
                        {
                            WhoseTurn = player_;
                            counterAttacks++;
                            return;
                        }
                    }
                }
            }
        }
        public void SetStatusPlayer() //устанавливается вначале раунда. атакующий есть только в начале раунда, после хода он становится подкидывающим.
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
            else if (Round.RoundStatus == RoundStatus.continues)
            {
                return;
            }
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
#endregion

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


