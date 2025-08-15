using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class Card
    {
        private int rank;
        private string suit;

        public Card(int rank, string suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        public int GetRank()
        {
            return rank;
        }

        public string GetSuit()
        {
            return suit;
        }

        public string GetImagePath()
        {
            string rankName;
            switch (rank)
            {
                case 11: rankName = "jack"; break;
                case 12: rankName = "queen"; break;
                case 13: rankName = "king"; break;
                case 14: rankName = "ace"; break;
                default: rankName = rank.ToString(); break;
            }
            return $"{UILayoutConfig.CardImagesFolder}/{rankName}_of_{suit}.png";
        }

        public string GetBackImagePath()
        {
            return $"{UILayoutConfig.CardImagesFolder}/back.png";
        }

        public override bool Equals(object obj)
        {
            if (obj is Card other)
                return this.rank == other.rank && this.suit == other.suit;

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + rank.GetHashCode();
                hash = hash * 23 + (suit != null ? suit.GetHashCode() : 0);
                return hash;
            }
        }
    }
}