using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp18
{
    public class CardPairOnDesk
    {
        public Card LargerCard { get; private set; }
        public Card LessCard { get; private set; }
        
        //public CardPairOnDesk()
        //{
        //    LargerCard = new Card();
        //    LessCard = new Card();
        //}

        public void Add(Card card)
        {
            if(LargerCard is null && LessCard is null)
            {
                LessCard = card;
            }

            else if(!(LessCard is null))
            {
                if(card > LessCard)
                {
                    LargerCard = card;
                }                
            }

            else
            {
                throw new Exception("нельзя отбиваться меньшей картой");
            }
        }

        public override string ToString()
        {
            if(LargerCard is null)
            {
                return $"{LessCard.ToString()}:";
            }
            return $"{LessCard.ToString()}:{LargerCard.ToString()}";
        }
    }
}
