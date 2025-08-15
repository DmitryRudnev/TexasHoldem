using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class HumanPlayer : Player
    {
        public HumanPlayer() : base("McLovin") { }

        public override void MakeMove(string action)
        {
            GameManager gameManager = GameManager.Instance();
            switch (action)
            {
                case "Call":
                    gameManager.SetPlayerCall(this);
                    break;
                case "Fold":
                    gameManager.SetPlayerFold(this);
                    break;
            }
            gameManager.SetNextState();
        }
    }
}