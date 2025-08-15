using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class BotPlayer : Player
    {
        private Random random;

        public BotPlayer(string botName) : base(botName)
        {
            random = new Random(Environment.TickCount ^ botName.GetHashCode());
        }

        public override void MakeMove(string action)
        {
            GameManager gameManager = GameManager.Instance();

            switch (action)
            {
                case "Bet":
                    int bet = random.Next(25, 50);
                    gameManager.SetPlayerBet(this, bet);
                    break;
                case "Random":
                    int rand = random.Next(0, 100);
                    if (rand < 90)
                    {
                        gameManager.SetPlayerCall(this);
                    }
                    else
                    {
                        gameManager.SetPlayerFold(this);
                    }
                    break;

            }
        }
    }
}
