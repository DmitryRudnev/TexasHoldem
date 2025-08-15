using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class PreflopState : IGameState
    {
        public void Handle(GameManager gameManager)
        {
            gameManager.ResetForNewGame();
            gameManager.ProcessPlayerMoves();
        }
    }
}