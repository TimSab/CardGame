using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    namespace ConsoleApp18
    {
        public class CardPairsOnDesk
        {
            public List<Dictionary<int, List<Card>>> CardPairs { get; set; }
            private Dictionary<int, List<Card>> cardPair;

            // минимальное значение keyOrValue равно 1.
            public CardPairsOnDesk(int keyOrValue, Card card)
            {
                CardPairs = new List<Dictionary<int, List<Card>>>();
                cardPair = new Dictionary<int, List<Card>>();
                if (keyOrValue % 2 != 0)
                {
                    cardPair[keyOrValue] = new List<Card>();
                    cardPair[keyOrValue].Add(card);
                }
                else if (keyOrValue % 2 == 0)
                {
                    cardPair[keyOrValue - 1].Add(card);
                }
            }
        }
    }
}
