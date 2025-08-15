using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class ShowdownState : IGameState
    {
        public void Handle(GameManager gameManager)
        {
            gameManager.AwardPotToWinner();
        }
    }
}