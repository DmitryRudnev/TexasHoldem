using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public interface IGameState
    {
        void Handle(GameManager gameManager);
    }
}