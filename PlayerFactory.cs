using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class PlayerFactory
    {
        public IPlayer CreatePlayer(string type, string botName="Bot")
        {
            if (type == "human")
            {
                return new HumanPlayer();
            }
            else if (type == "bot")
            {
                return new BotPlayer(botName);
            }
            else
            {
                return null;
            }
        }
    }
}