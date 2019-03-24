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

        public void StartRound(RoundEndResult? lastRoundResult, DurakGame game)
        {
            LastRoundResult = lastRoundResult;
            RoundStatus = RoundStatus.continues;            
            game.SetStatusPlayer();
            game.GetFirstThrowsUpPlayer();

            game.HandOutCards(game.Players);
        }
        
        public void EndRound(RoundEndResult? roundEndResult)
        {
            RoundEndResult = roundEndResult;
            RoundStatus = RoundStatus.over;
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
}
