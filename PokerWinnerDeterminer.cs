using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class PokerWinnerDeterminer
    {
        private List<IPlayer> players;
        private List<Card> communityCards;
        private List<PokerHand> playerHands;
        private IPlayer winner;

        public PokerWinnerDeterminer(List<IPlayer> players, List<Card> communityCards)
        {
            this.players = players;
            this.communityCards = communityCards;
            playerHands = new List<PokerHand>();
        }

        public void SetWinner(IPlayer winner)
        {
            this.winner = winner;
        }
        public IPlayer GetWinner()
        {
            return winner;
        }

        public void SetCommunityCards(List<Card> communityCards)
        {
            this.communityCards = new List<Card>(communityCards);
        }
        public List<PokerHand> GetPlayerHands()
        {
            return playerHands;
        }

        public void DetermineWinner()
        {
            playerHands.Clear();
            PokerHand strongestHand = new PokerHand();
            winner = players[0];
            List<Card> seven = new List<Card>(communityCards);
            foreach (IPlayer player in players)
            {
                PokerHand hand = new PokerHand();
                playerHands.Add(hand);
                if (player.GetActive())
                {
                    seven.AddRange(player.GetHand());
                    hand.SetSeven(seven);

                    if (hand.StrongerThan(strongestHand))
                    {
                        strongestHand = hand;
                        winner = player;
                    }
                    seven.RemoveAt(6);
                    seven.RemoveAt(5);
                }
            }
        }
    }
}
