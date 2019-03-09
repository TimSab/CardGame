using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public static class CardComparer
    {
        public static CardSuit? TrumpSuit {get; set;}

        private static bool IsTrump(Card card)
        {
            return TrumpSuit == card.Suit;
        }

        public static int Compare(Card card1, Card card2)
        {
            //if (card1 is null && card2 is null) return null;
            //if (card1 is null) return -1;
            //if (card2 is null) return 1;
            if (TrumpSuit is null)
            {
                throw new Exception("в игре нет козыря");
            }

            if (card1.Suit == card2.Suit)
            {
                if (card1.Value == card2.Value) return 0;
                if (card1.Value > card2.Value) return 1;
                return -1;
            }
            else if (IsTrump(card1)) return 1;
            else if (IsTrump(card2)) return -1;

            throw new Exception("Разная масть");
        }
    }
}
