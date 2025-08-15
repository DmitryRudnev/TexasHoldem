using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TexasHoldem
{
    public class PokerHand
    {
        private List<Card> seven;
        private int combinationRank;
        private List<Card> combinationCards;
        private List<Card> kickers;

        public PokerHand()
        {
            seven = new List<Card>();
            combinationRank = 0;
            combinationCards = new List<Card>();
            kickers = new List<Card>();
        }

        public void SetSeven(List<Card> seven) 
        {
            this.seven = new List<Card>(seven);
            DetermineBestCards();
        }

        public List<Card> GetSeven() { return seven; }
        public int GetCombinationRank() { return combinationRank; }
        public List<Card> GetCombinationCards() { return combinationCards; }
        public List<Card> GetKickers() { return kickers; }
        public string GetCombinationName() 
        {  
            switch (combinationRank)
            {
                case 10: return "Роял Флеш";
                case 9: return "Стрит Флеш";
                case 8: return "Каре";
                case 7: return "Фул Хаус";
                case 6: return "Флеш";
                case 5: return "Стрит";
                case 4: return "Сет";
                case 3: return "Две Пары";
                case 2: return "Пара";
                case 1: return "Старшая карта";
            }
            return "Нет";
        }



        public bool StrongerThan(PokerHand other)
        {
            if (other.GetSeven().Count == 0) return true;

            int otherCombinationRank = other.GetCombinationRank();
            if (combinationRank > otherCombinationRank) return true;
            if (combinationRank < otherCombinationRank) return false;

            List<Card> otherCombinationCards = other.GetCombinationCards();
            for (int i = 0; i < combinationCards.Count; i++)
            {
                if (combinationCards[i].GetRank() > otherCombinationCards[i].GetRank()) return true;
                if (combinationCards[i].GetRank() < otherCombinationCards[i].GetRank()) return false;
            }

            List<Card> otherKickers = other.GetKickers();
            for (int i = 0; i < kickers.Count; i++)
            {
                if (kickers[i].GetRank() > otherKickers[i].GetRank()) return true;
                if (kickers[i].GetRank() < otherKickers[i].GetRank()) return false;
            }
            return false;
        }


        private void DetermineBestCards()
        {
            seven.Sort((card1, card2) => card2.GetRank().CompareTo(card1.GetRank()));

            if (RoyalFlush(seven).Count != 0)
            {
                combinationRank = 10;
                combinationCards = RoyalFlush(seven);
            }

            else if (SraightFlush(seven).Count != 0) 
            {
                combinationRank = 9;
                combinationCards = SraightFlush(seven);
            }

            else if (FourOfAKind(seven).Count != 0)
            {
                combinationRank = 8;
                combinationCards = FourOfAKind(seven);
                kickers = DetermineKickers(FourOfAKind(seven));
            }

            else if (FullHouse(seven).Count != 0)
            {
                combinationRank = 7;
                combinationCards = FullHouse(seven);
            }

            else if (Flush(seven).Count != 0)
            {
                combinationRank = 6;
                combinationCards = Flush(seven);
            }

            else if (Straight(seven).Count != 0)
            {
                combinationRank = 5;
                combinationCards = Straight(seven);
            }

            else if (ThreeOfAKind(seven).Count != 0)
            {
                combinationRank = 4;
                combinationCards = ThreeOfAKind(seven);
                kickers = DetermineKickers(ThreeOfAKind(seven));
            }

            else if (TwoPair(seven).Count != 0)
            {
                combinationRank = 3;
                combinationCards = TwoPair(seven);
                kickers = DetermineKickers(TwoPair(seven));
            }

            else if (OnePair(seven).Count != 0)
            {
                combinationRank = 2;
                combinationCards = OnePair(seven);
                kickers = DetermineKickers(OnePair(seven));
            }

            else
            {
                combinationRank = 1;
                combinationCards = HighCard(seven);
                kickers = DetermineKickers(HighCard(seven));
            }
        }



        // Метод каждой комбинации возвращает пустой массив, если соответствующаяя комбинация не может быть составлена
        // Иначе возвращает массив с картами 
        private List<Card> HighCard(List<Card> cards)
        {
            return new List<Card> { cards[0] };
        }

        private List<Card> OnePair(List<Card> cards)
        {
            for (int i = 0; i < cards.Count-1; i++)
                if (cards[i].GetRank() == cards[i+1].GetRank())
                    return new List<Card> { cards[i], cards[i+1] };
            
            return new List<Card>();
        }

        private List<Card> TwoPair(List<Card> cards)
        {
            List<Card> result = new List<Card>();
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].GetRank() == cards[i + 1].GetRank())
                {
                    result.Add(cards[i]);
                    result.Add(cards[i+1]);
                    if (result.Count == 4) return result;
                    i++;
                }
            }
            return new List<Card>();
        }

        private List<Card> ThreeOfAKind(List<Card> cards)
        {
            for (int i = 0; i < cards.Count - 2; i++)
                if (cards[i].GetRank() == cards[i + 1].GetRank() && cards[i].GetRank() == cards[i + 2].GetRank())
                    return new List<Card> { cards[i], cards[i + 1], cards[i + 2] };

            return new List<Card>();
        }

        private List<Card> Straight(List<Card> cards)
        {
            List<Card> temp = new List<Card> { cards[0] };
            for (int i = 1; i < cards.Count; i++)
                if (cards[i].GetRank() != temp[temp.Count - 1].GetRank())
                    temp.Add(cards[i]);

            if (temp.Count >= 5)
            {
                int r0, r1, r2, r3, r4;
                for (int i = 0; i < temp.Count - 4; i++)
                {
                    r0 = temp[i].GetRank();
                    r1 = temp[i + 1].GetRank();
                    r2 = temp[i + 2].GetRank();
                    r3 = temp[i + 3].GetRank();
                    r4 = temp[i + 4].GetRank();
                    if (r0 == r1 + 1 && r0 == r2 + 2 && r0 == r3 + 3 && r0 == r4 + 4)
                        return new List<Card> { temp[i], temp[i + 1], temp[i + 2], temp[i + 3], temp[i + 4] };
                }
            }
            return new List<Card>();
        }

        private List<Card> Flush(List<Card> cards)
        {
            List<Card> temp = new List<Card>(cards);
            temp.Sort((card1, card2) => card1.GetSuit().CompareTo(card2.GetSuit()));
            
            string s0, s1, s2, s3, s4;
            for (int i = 0; i < temp.Count-4; i++)
            {
                s0 = temp[i].GetSuit();
                s1 = temp[i + 1].GetSuit();
                s2 = temp[i + 2].GetSuit();
                s3 = temp[i + 3].GetSuit();
                s4 = temp[i + 4].GetSuit();
                if (s0 == s1 && s0 == s2 && s0 == s3 && s0 == s4)
                    return new List<Card> { temp[i], temp[i + 1], temp[i + 2], temp[i + 3], temp[i + 4] };
            }
            return new List<Card>();
        }

        private List<Card> FullHouse(List<Card> cards)
        {
            List<Card> fullHouse = ThreeOfAKind(cards);
            if (fullHouse.Count != 0)
            {
                List<Card> temp = new List<Card>(cards);
                foreach (Card card in fullHouse) 
                    temp.Remove(card);
                
                List<Card> onePair = OnePair(temp);
                if (onePair.Count != 0)
                {
                    fullHouse.AddRange(onePair);
                    return fullHouse;
                }   
            }
            return new List<Card>();
        }

        private List<Card> FourOfAKind(List<Card> cards)
        {
            cards.Sort((card1, card2) => card2.GetRank().CompareTo(card1.GetRank()));
            for (int i = 0; i < cards.Count - 3; i++)
                if (cards[i].GetRank() == cards[i + 1].GetRank() && cards[i].GetRank() == cards[i + 2].GetRank() && cards[i].GetRank() == cards[i + 3].GetRank())
                    return new List<Card> { cards[i], cards[i + 1], cards[i + 2], cards[i + 3] };
            return new List<Card>();
        }

        private List<Card> SraightFlush(List<Card> cards)
        {
            cards.Sort((card1, card2) => card2.GetRank().CompareTo(card1.GetRank()));
            List<Card> temp = new List<Card> { cards[0] };
            for (int i = 1; i < cards.Count; i++)
                if (temp[temp.Count - 1].GetRank() != cards[i].GetRank())
                    temp.Add(cards[i]);

            if (temp.Count >= 5)
            {
                int r0, r1, r2, r3, r4;
                string s0, s1, s2, s3, s4;
                bool stright, flush;
                for (int i = 0; i < temp.Count - 4; i++)
                {
                    r0 = temp[i].GetRank();
                    r1 = temp[i + 1].GetRank();
                    r2 = temp[i + 2].GetRank();
                    r3 = temp[i + 3].GetRank();
                    r4 = temp[i + 4].GetRank();
                    s0 = cards[i].GetSuit();
                    s1 = cards[i + 1].GetSuit();
                    s2 = cards[i + 2].GetSuit();
                    s3 = cards[i + 3].GetSuit();
                    s4 = cards[i + 4].GetSuit();
                    stright = r0 == r1 + 1 && r0 == r2 + 2 && r0 == r3 + 3 && r0 == r4 + 4;
                    flush = s0 == s1 && s0 == s2 && s0 == s3 && s0 == s4;
                    if (stright && flush)
                        return new List<Card> { temp[i], temp[i + 1], temp[i + 2], temp[i + 3], temp[i + 4] };
                }
            }
            return new List<Card>();
        }

        private List<Card> RoyalFlush(List<Card> cards)
        {
            List<Card> temp = SraightFlush(cards);
            if (temp.Count != 0)
                if (temp[0].GetRank() == 14)
                    return temp;
            
            return new List<Card>();
        }

        private List<Card> DetermineKickers(List<Card> combinationCards)
        {
            List<Card> kickers = new List<Card>();
            List<Card> temp = new List<Card>(seven);
            temp.Sort((card1, card2) => card2.GetRank().CompareTo(card1.GetRank()));

            foreach (Card card in combinationCards) 
                temp.Remove(card);

            for (int i = 0; i < (5 - combinationCards.Count); i++)
                kickers.Add(temp[i]);
            
            return kickers;
        }
    }
}
