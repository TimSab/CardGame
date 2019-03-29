using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class Round
    {
        public RoundEndResult? LastRoundResult { get; set; }
        public RoundStatus RoundStatus { get; set; }
        public RoundEndResult? RoundEndResult { get; set; }
        public Player DefencePlayer { get; set; }
        public List<Player> ListThrowPlayers { get; set; }
        public Player WhoseTurn { get; set; }

        public void StartRound(DurakGame game)
        {
            LastRoundResult = RoundEndResult;
            RoundStatus = RoundStatus.continues;
            WhoseTurn = DecideWhoseTurn(game);
            game.SetStatusPlayer();
            foreach (var player in game.Players)
            {
                if (player.StatusPlayer == StatusPlayer.defence)
                {
                    DefencePlayer = player;
                    break;
                }
            }
            ListThrowPlayers = GetNewQueueThrowPlayers(game);
            ListThrowPlayers.Add(game.Players.Where(p => p.StatusPlayer == StatusPlayer.attack).First());

            game.HandOutCards();
        }

        public void EndRound()
        {
            RoundStatus = RoundStatus.over;
        }

        public List<Player> GetNewQueueThrowPlayers(DurakGame game)
        {
            var listThrowPlayer = new List<Player>();
            var throwPlayers = game.Players.Where(p => p.StatusPlayer == StatusPlayer.throwsUp).ToArray();
            for (int i = DefencePlayer.ConnectionNumber + 1; i <= DefencePlayer.ConnectionNumber + throwPlayers.Length; i++)
            {
                foreach (var player in throwPlayers)
                {
                    if (player.ConnectionNumber == i % (game.Players.Count + 1))
                    {
                        listThrowPlayer.Add(player);
                    }
                }
            }
            return listThrowPlayer;
        }

        public List<Player> GetNewQueueThrowPlayers(DurakGame game, Player firstInNewListThrowPlayers)
        {
            var listThrowPlayer = new List<Player>();
            var throwPlayers = game.Players.Where(p => p.StatusPlayer == StatusPlayer.throwsUp).ToArray();

            for (int i = firstInNewListThrowPlayers.ConnectionNumber + 1; i <= firstInNewListThrowPlayers.ConnectionNumber + throwPlayers.Length; i++)
            {
                foreach (var player in throwPlayers)
                {
                    if (player.ConnectionNumber == i % (game.Players.Count + 1))
                    {
                        listThrowPlayer.Add(player);
                    }
                }
            }
            return listThrowPlayer;
        }

        public Player DecideWhoseTurn(DurakGame game)
        {
            foreach (var player_ in game.Players)
            {
                if (player_.StatusPlayer == StatusPlayer.attack)
                {
                    return player_;
                }
            }
            Player firstPlayer = null;
            var fufel = true;
            Card minTrumpCard = new Card(game.Deck.TrumpSuit, CardValue.Ace);
            //масть не имеет значение.
            Card maxNotTrump = new Card(CardSuit.Club, CardValue.Six);

            foreach (var player in game.Players)
            {
                var GetMaxNotTrumpOrMinTrumpPlayer = game.GetMaxNotTrumpOrMinTrump(player);
                if (GetMaxNotTrumpOrMinTrumpPlayer.Suit == game.Deck.TrumpSuit)      // Окей, две ОДИНАКОВЫХ КОЗЫРКИ у разных игроков
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

            firstPlayer.StatusPlayer = StatusPlayer.attack;
            return firstPlayer;
        }
    }
}
public enum RoundEndResult
{
    defended,
    notDefended,
}

public enum RoundStatus
{
    continues,
    over
}

