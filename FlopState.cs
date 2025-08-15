using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class FlopState : IGameState
    {
        public void Handle(GameManager gameManager)
        {
            gameManager.DealCommunityCards(3);
            gameManager.ProcessPlayerMoves();
        }
    }
}