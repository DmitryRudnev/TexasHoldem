using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class UIController : IGameObserver
    {
        private GameForm form;

        public UIController(GameForm form)
        {
            this.form = form;
        }

        public void Update(IGameState state, string update, int playerIndex, string action)
        {
            form.UpdateUI(state, update, playerIndex, action);
        }
    }
}