using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public interface IGameObserver
    {
        void Update(IGameState state, string update, int playerIndex, string action);
    }
}