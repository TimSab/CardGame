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
        public StatusPlayer StatusPlayer { get; set; }

        public Player(string name)
        {
            Hand = new List<Card>();
            Name = name;
            StatusPlayer = StatusPlayer.awaitStatus;
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

        public void Pass()
        {
            
        }

        public void GetAll(List<CardPairOnDesk> cardPairsOnDesk)
        {
            foreach (var cardPairOnDesk in cardPairsOnDesk)
            {
                if (!(cardPairOnDesk.LargerCard is null))
                {
                    var largerCard = new Card(cardPairOnDesk.LargerCard.Suit, cardPairOnDesk.LargerCard.Value);
                    Hand.Add(largerCard);
                }

                if (!(cardPairOnDesk.LessCard is null))
                {
                    var lessCard = new Card(cardPairOnDesk.LessCard.Suit, cardPairOnDesk.LessCard.Value);
                    Hand.Add(lessCard);
                }
            }
            cardPairsOnDesk.RemoveRange(0, cardPairsOnDesk.Count);

        }
    }

    public enum StatusPlayer
    {
        throwsUp,                           // подкидывает   ПРИ ЭТОМ СТАТУСЕ ИГРОК МОЖЕТ ПРОПУСТИТЬ ХОД ИЛИ ПОДКИНУТЬ КАРТУ.
        defence,                        // отбивается    ПРИ ЭТОМ СТАТУСЕ ИГРОК ОБЯЗАН ПОКРЫТЬ КАРТУ, ЛИБО ЗАБРАТЬ ВСЕ КАРТЫ СО СТОЛА.
        attack,                                 // ходит(когда уже все карты биты, когда на столе нет карт) ПРИ ЭТОМ СТАТУСЕ ИГРОК ОБЯЗАН ХОДТЬ.
        awaitStatus                               // ожидает       ПРИ ЭТОМ СТАТУСЕ ИГРОК МОЖЕТ ПРОПУСТИТЬ ХОД.
    }
}
