using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class Card
    {
        public CardValue Value { get; set; }
        public CardSuit Suit { get; set; }

        public Card(CardSuit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public bool IsTrump(CardSuit trump)
        {
            return Suit == trump;
        }
        
        public static bool operator >(Card card1, Card card2)
        {
            switch (CardComparer.Compare(card1, card2))
            {                
                case 1:
                    return true;
                case 0:
                case -1:
                default:
                    return false;                
            }
        }

        public static bool operator <(Card card1, Card card2)
        {
            switch (CardComparer.Compare(card1, card2))
            {
                case -1:
                    return true;
                case 0:
                case 1:
                default:
                    return false;
            }
        }

        public static bool operator !=(Card card1, Card card2)
        {
            switch (CardComparer.Compare(card1, card2))
            {
                case 0:
                    return false;
                case -1:
                case 1:
                default:
                    return true;
            }
        }

        public static bool operator ==(Card card1, Card card2)
        {
            switch (CardComparer.Compare(card1, card2))
            {
                case 0:
                    return true;
                case -1:
                case 1:
                default:
                    return false;
            }
        }

        public static bool operator >=(Card card1, Card card2)
        {
            switch (CardComparer.Compare(card1, card2))
            {
                case 0:
                case 1:
                    return true;
                case -1:                
                default:
                    return false;
            }            
        }

        public static bool operator <=(Card card1, Card card2)
        {
            switch (CardComparer.Compare(card1, card2))
            {
                case 0:
                case -1:
                    return true;                
                case 1:
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return $"{Value} {Suit}";
        }
    }

    public enum CardSuit
    {
        Diamond,
        Heart,
        Spade,
        Club
    }

    public enum CardValue
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
        Joker
    }

    enum CardPosition
    {
        Player,
        Desc,
        Deck,
        OutOfDesc
    }
}
