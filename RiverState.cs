using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class RiverState : IGameState
    {
        public void Handle(GameManager gameManager)
        {
            gameManager.DealCommunityCards(1);
            gameManager.ProcessPlayerMoves();
        }
    }
}