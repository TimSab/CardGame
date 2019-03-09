using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class Player
    {
        public List<Card> Hand { get; set; }
        public string Name { get; set; }
        public int ConnectionNumber { get; set; }

        public Player(string name)
        {
            Hand = new List<Card>();
            Name = name;
        }

        public Card Throw(Card card)
        {
            if (Hand.Contains(card))
            {
                Hand.Remove(card);
                return card;
            }

            throw new ArgumentException($"у игрока {Name} нет карты {card}");
        }

        public Card Throw(int numberCard)
        {
            if (Hand.Count > numberCard)
            {
                var card = Hand[numberCard];
                Hand.RemoveAt(numberCard);
                return card;
            }

            throw new ArgumentException($"у игрока {Name} нет карты {Hand[numberCard -1]}");
        }

        public void OnPass()
        {
            Pass?.Invoke();
        }

        public void GetAll()
        {
            
        }

        public event Action Pass; // добавить метод который переводит указатель очереди на следующего игрока.
    }
}
