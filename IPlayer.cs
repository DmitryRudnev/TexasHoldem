using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TexasHoldem
{
    public interface IPlayer
    {
        string GetName();
        int GetDeposit();
        bool GetActive();
        Card[] GetHand();
        string GetVictoryGifPath();
        string GetVictoryAudioPath();
        string GetVictoryVideoPath();

        void SetName(string name);
        void SetDeposit(int deposit);
        void SetActive(bool active);
        void SetHand(Card card1, Card card2);

        void MakeMove();
        void MakeMove(string action);
    }
}