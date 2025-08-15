using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem
{
    public class Player : IPlayer
    {
        private string name;
        private int deposit;
        private bool active; 
        private Card[] hand;

        public Player(string playerName)
        {
            name = playerName;
            deposit = 1000;
            active = true;
            hand = new Card[2];
        }

        public string GetName() { return name; }
        public int GetDeposit() { return deposit; }
        public bool GetActive() { return active; }
        public Card[] GetHand() { return hand; }
        public string GetVictoryGifPath() { return UILayoutConfig.VictoryAnimationGifsFolder + '/' + name + ".gif"; }
        public string GetVictoryAudioPath() { return UILayoutConfig.VictoryAnimationAudiosFolder + '/' + name + ".WAV"; }
        public string GetVictoryVideoPath() { return UILayoutConfig.VictoryAnimationVideosFolder + '/' + name + ".webm"; }


        public void SetName(string name) { this.name = name; }
        public void SetDeposit(int deposit) { this.deposit = deposit; }
        public void SetActive(bool active) { this.active = active; }
        public void SetHand(Card card1, Card card2)
        {
            hand[0] = card1;
            hand[1] = card2;
        }

        public virtual void MakeMove() { }
        public virtual void MakeMove(string action) { }
    }
}
