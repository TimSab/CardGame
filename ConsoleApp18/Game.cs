using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public abstract class Game
    {
        public abstract Deck Deck { get; set; }
        public abstract List<Player> Players { get; set; }

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

        public abstract void StartGame();
        public abstract void StopGame();
        public abstract void СhangeWhoseTurn();
        public abstract void HandOutCards();
    }
}
