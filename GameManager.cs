using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Linq;
using System.Collections;

namespace TexasHoldem
{
    public class GameManager
    {
        private static GameManager instance = null;
        private List<Card> deck; 
        private List<IPlayer> players;
        private List<Card> communityCards;
        private PokerWinnerDeterminer pokerJudge;
        private IGameState currentState;
        private List<IGameObserver> observers;
        private Random random;
        private int currentBet;
        private int pot;
        private int delay;

        private GameManager()
        {
            deck = new List<Card>(); 
            players = new List<IPlayer>();
            communityCards = new List<Card>();
            pokerJudge = new PokerWinnerDeterminer(players, communityCards);
            observers = new List<IGameObserver>();
            currentState = new PreflopState();
            random = new Random();
            currentBet = 0;
            pot = 0;
            delay = 750;
        }
        public static GameManager Instance()
        {
            if (instance == null) { instance = new GameManager(); }
            return instance;
        }




        // ГЕТТЕРЫ
        public int GetPlayerDeposit(IPlayer player) { return player.GetDeposit(); }
        public bool GetPlayerActive(IPlayer player) { return player.GetActive(); }
        public int GetPot() { return pot; }
        public int GetBet() { return currentBet; }
        public List<IPlayer> GetPlayers() { return players; }
        public int GetPlayersCount() { return players.Count; }
        public List<Card> GetCommunityCards() { return communityCards; }
        public List<PokerHand> GetPlayerCombinations() { return pokerJudge.GetPlayerHands(); }
        public IPlayer GetWinner() { return pokerJudge.GetWinner(); }
        public IGameState GetState() { return currentState; }
        public void AddPlayer(IPlayer player) { players.Add(player); }
        public void AddObserver(IGameObserver observer) { observers.Add(observer); }



        public void NotifyObservers(string update="", int playerIndex=0, string action="")
        {
            foreach (IGameObserver observer in observers)
                observer.Update(currentState, update, playerIndex, action);
        }





        // ДЛЯ НОВОЙ ИГРЫ
        public void InitializeDeck()
        {
            deck.Clear();
            string[] suits = { "hearts", "spades", "clubs", "diamonds" };
            for (int rank = 2; rank <= 14; rank++)
            {
                foreach (string suit in suits)
                {
                    deck.Add(new Card(rank, suit));
                }
            }
        }
        private Card GetRandomCard()
        {
            int index = random.Next(0, deck.Count);
            Card card = deck[index];
            deck.RemoveAt(index);
            return card;
        }
        public void DealPlayerCards()
        {
            foreach (IPlayer player in players)
            {
                Card card1 = GetRandomCard();
                Card card2 = GetRandomCard();
                player.SetHand(card1, card2);
            }
        }
        public void DealCommunityCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Card card = GetRandomCard();
                communityCards.Add(card);
            }
            NotifyObservers();
        }
        public void ResetForNewGame()
        {
            InitializeDeck();
            DealPlayerCards();
            foreach (IPlayer player in players)
            {
                player.SetActive(true);
            }
            communityCards.Clear();
            currentBet = 0;
            pot = 0;
            NotifyObservers();
        }





        public void SetState(IGameState state)
        {
            currentState = state;
            currentState.Handle(this);
        }
        public void SetNextState()
        {
            if (currentState is PreflopState) SetState(new FlopState());
            else if (currentState is FlopState) SetState(new TurnState());
            else if (currentState is TurnState) SetState(new RiverState());
            else if (currentState is RiverState) SetState(new ShowdownState());
        }
        public void SetPlayerBet(IPlayer player, int amount)
        {
            currentBet = amount;
            int playerCurrentDeposit = player.GetDeposit();
            int playerNewDeposit = playerCurrentDeposit - currentBet;
            player.SetDeposit(playerNewDeposit);
            pot += currentBet;
            int playerIndex = players.IndexOf(player);
            NotifyObservers("Player Made Move", playerIndex, "Bet");
        }
        public void SetPlayerCall(IPlayer player)
        {
            int playerCurrentDeposit = player.GetDeposit();
            int playerNewDeposit = playerCurrentDeposit - currentBet;
            player.SetDeposit(playerNewDeposit);
            pot += currentBet;
            int playerIndex = players.IndexOf(player);
            NotifyObservers("Player Made Move", playerIndex, "Call");
        }
        public void SetPlayerFold(IPlayer player)
        {
            player.SetActive(false);
            int playerIndex = players.IndexOf(player);
            NotifyObservers("Player Made Move", playerIndex, "Fold");
        }
        public async void ProcessPlayerMoves()
        {
            await Task.Delay(delay); 
            for (int i = 1; i < players.Count; i++)
            {
                IPlayer player = players[i];
                if (GetPlayerActive(player))
                {
                    if (i == 1) { player.MakeMove("Bet"); }
                    else { player.MakeMove("Random"); }
                    await Task.Delay(delay);
                }
            }
            if (players[0].GetActive()) NotifyObservers("Awaiting For User Move");
            else SetNextState();
        }
        public void AwardPotToWinner()
        {
            pokerJudge.DetermineWinner();
            IPlayer winner = pokerJudge.GetWinner();
            winner.SetDeposit(winner.GetDeposit() + pot);
            pot = 0;
            NotifyObservers();
        }
    }
}
