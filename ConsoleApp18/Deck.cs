using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class Deck
    {
        public Stack<Card> Cards { get; private set; }
        public CardSuit TrumpSuit { get; private set; }

        public Deck(int countDeck)
        {
            Cards = new Stack<Card>(countDeck);
            if(countDeck == 36)
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    for (int i = 4; i < 13; i++)
                    {
                        Cards.Push(new Card(suit, (CardValue)i));
                    }
                }

            else
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    Cards.Push(new Card(suit, value));
                }
            }

            Shuffle();
            TrumpSuit = GetTrumpSuit();
        }

        public void Shuffle()
        {
            var rand = new Random();

            var cardArr = Cards.ToArray();

            for (int i = cardArr.Count() - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                Swap(cardArr, i, j);
            }

            Cards.Clear();

            foreach (var card in cardArr)
            {
                Cards.Push(card);
            }
        }

        private void Swap(Array arr, int i, int j)
        {
            var tmp = arr.GetValue(j);
            arr.SetValue(arr.GetValue(i), j);
            arr.SetValue(tmp, i);
        }

        private CardSuit GetTrumpSuit()
        {
            return Cards.Last().Suit;
        }

    }
}
