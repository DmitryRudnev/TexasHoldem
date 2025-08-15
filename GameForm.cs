using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TexasHoldem.UILayoutConfig;
using Microsoft.Web.WebView2.WinForms;
using NAudio.Wave;



namespace TexasHoldem
{
    public partial class GameForm : Form
    {
        private GameManager gameManager;
        private UIController uiController;
        private PlayerFactory playerFactory;

        private Button startButton;
        private Button callButton;
        private Button foldButton;
        private Button stopAnimationButton;

        private PictureBox[] playerCardBoxes;
        private PictureBox[] communityCardBoxes;
        private PictureBox[] actionIconBoxes;

        private Label[] depositLabels;
        private Label[] combinationLabels;
        private Label potLabel;
        private Label betLabel;
        private Label testLabel;

        private WebView2 webView2;
        private AudioFileReader audioFile;
        private WaveOutEvent audioPlayer;



        public GameForm()
        {
            InitializeComponent();
            Size = new Size(WindowWidth, WindowHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Icon = new Icon(IconImagePath);
            Text = "Texas Hold'em";
            BackgroundImage = Image.FromFile(TableImagePath);
            BackgroundImageLayout = ImageLayout.Stretch;

            gameManager = GameManager.Instance();
            uiController = new UIController(this);
            playerFactory = new PlayerFactory();

            playerCardBoxes = new PictureBox[12];
            communityCardBoxes = new PictureBox[5];
            actionIconBoxes = new PictureBox[6];

            depositLabels = new Label[6];
            combinationLabels = new Label[6];

            InitializePlayers();
            InitializeButtons();
            InitializePictures();
            InitializeLabels();
            gameManager.AddObserver(uiController);
            ShowStartButton();

            InitializeWebView2();
            audioPlayer = new WaveOutEvent();
        }



        private void InitializePlayers()
        {
            gameManager.AddPlayer(playerFactory.CreatePlayer("human"));
            gameManager.AddPlayer(playerFactory.CreatePlayer("bot", "Patrick"));
            gameManager.AddPlayer(playerFactory.CreatePlayer("bot", "Jason"));
            gameManager.AddPlayer(playerFactory.CreatePlayer("bot", "Dwayne"));
            gameManager.AddPlayer(playerFactory.CreatePlayer("bot", "John"));
            gameManager.AddPlayer(playerFactory.CreatePlayer("bot", "Ryan"));
        }



        // КНОПКИ
        private Button CreateButton(int x, int y, int width, int height, string text, EventHandler onClick, Color color, Color foreColor)
        {
            Button button = new Button();
            button.Location = new Point(x, y);
            button.Size = new Size(width, height);
            button.Visible = false;
            button.Text = text;
            button.Font = new Font("Microsoft Sans Serif", DefaultFontSize, FontStyle.Regular);
            button.BackColor = color;
            button.ForeColor = foreColor;
            button.Click += onClick;
            Controls.Add(button);
            return button;
        }
        private void InitializeButtons()
        {
            startButton = CreateButton(StartButtonX, StartButtonY, StartButtonWidth, StartButtonHeihgt, "Start Game", StartButton_Click, Color.WhiteSmoke, Color.Black);
            callButton = CreateButton(CallButtonX, ActionButtonY, ActionButtonWidth, DefaultHeight, "Call", CallButton_Click, Color.LightGreen, Color.Black);
            foldButton = CreateButton(FoldButtonX, ActionButtonY, ActionButtonWidth, DefaultHeight, "Fold", FoldButton_Click, Color.PaleVioletRed, Color.Black);
            stopAnimationButton = CreateButton(1490, 25, DefaultHeight, DefaultHeight, "X", StopAnimationButton_Click, Color.WhiteSmoke, Color.Black);
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            HideStartButton();
            gameManager.SetState(new PreflopState());
        }
        private void CallButton_Click(object sender, EventArgs e)
        {
            HideActionButtons();
            HideBetLabel();
            IPlayer humanPlayer = gameManager.GetPlayers()[0];
            humanPlayer.MakeMove("Call");
        }
        private void FoldButton_Click(object sender, EventArgs e)
        {
            HideActionButtons();
            HideBetLabel();
            IPlayer humanPlayer = gameManager.GetPlayers()[0];
            humanPlayer.MakeMove("Fold");
        }
        private void StopAnimationButton_Click(object sender, EventArgs e)
        {
            if (audioPlayer.PlaybackState == PlaybackState.Playing)
                audioPlayer.Stop();
        }
        private void ShowStartButton() { startButton.Visible = true; }
        private void HideStartButton() { startButton.Visible = false; }
        private void ShowActionButtons()
        {
            callButton.Visible = true;
            foldButton.Visible = true;
        }
        private void HideActionButtons()
        {
            callButton.Visible = false;
            foldButton.Visible = false;
        }



        // ИЗОБРАЖЕНИЯ
        private PictureBox CreatePicture(int x = 0, int y = 0, int width = 0, int height = 0, PictureBoxSizeMode sizeMode = PictureBoxSizeMode.StretchImage)
        {
            PictureBox box = new PictureBox();
            box.Location = new Point(x, y);
            box.Size = new Size(width, height);
            box.SizeMode = sizeMode;
            box.BackColor = Color.Transparent;
            Controls.Add(box);
            return box;
        }
        private void InitializePictures()
        {
            for (int i = 0; i < gameManager.GetPlayersCount(); i++)
            {
                playerCardBoxes[i * 2] = CreatePicture(PlayerCard1X[i], PlayerCardsY[i], CardWidth, CardHeight);
                playerCardBoxes[i * 2 + 1] = CreatePicture(PlayerCard2X[i], PlayerCardsY[i], CardWidth, CardHeight);
                actionIconBoxes[i] = CreatePicture(IconX[i], IconY[i], IconWidth, IconHeight);
            }
            for (int i = 0; i < 5; i++)
                communityCardBoxes[i] = CreatePicture(CommunityCardX[i], CommunityCardY, CardWidth, CardHeight);
        }
        private void ShowCard(PictureBox card, string imagePath)
        {
            card.Image = Image.FromFile(imagePath);
            card.Visible = true;
        }
        private void ShowCommunityCard(int cardIndex)
        {
            List<Card> communityCards = gameManager.GetCommunityCards();
            string imagePath = communityCards[cardIndex].GetImagePath();
            ShowCard(communityCardBoxes[cardIndex], imagePath);

        }
        private void HideCommunityCards()
        {
            for (int i = 0; i < 5; i++)
                communityCardBoxes[i].Visible = false;
        }
        private void ShowPlayerCards(int playerIndex)
        {
            IPlayer player = gameManager.GetPlayers()[playerIndex];
            Card[] playerCards = player.GetHand();
            string card1Path = playerCards[0].GetImagePath();
            string card2Path = playerCards[1].GetImagePath();
            ShowCard(playerCardBoxes[playerIndex * 2], card1Path);
            ShowCard(playerCardBoxes[playerIndex * 2 + 1], card2Path);
        }
        private void HidePlayerCards(int playerIndex)
        {
            playerCardBoxes[playerIndex * 2].Image = Image.FromFile(CardBackPath);
            playerCardBoxes[playerIndex * 2 + 1].Image = Image.FromFile(CardBackPath);
        }
        private void HidePlayersCards()
        {
            for (int i = 0; i < gameManager.GetPlayersCount(); i++)
                HidePlayerCards(i);
        }
        private void ShowActionIcon(int playerIndex, string iconPath)
        {
            actionIconBoxes[playerIndex].Image = Image.FromFile(iconPath);
            actionIconBoxes[playerIndex].Visible = true;
        }
        private void HideActionIcon(int playerIndex)
        {
            actionIconBoxes[playerIndex].Visible = false;
        }
        private void HideActionIcons()
        {
            for (int i = 0; i < gameManager.GetPlayersCount(); i++)
                HideActionIcon(i);
        }



        // МЕТКИ
        private Label CreateLabel(int x, int y, int width = DefaultWidth, int height = DefaultHeight, int fontSize = DefaultFontSize)
        {
            Label label = new Label();
            label.Location = new Point(x, y);
            label.Size = new Size(width, height);
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.White;
            label.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            Controls.Add(label);
            return label;
        }
        private void InitializeLabels()
        {
            for (int i = 0; i < gameManager.GetPlayersCount(); i++)
            {
                depositLabels[i] = CreateLabel(PlayerLabelX[i], PlayerLabelY[i]);
                combinationLabels[i] = CreateLabel(CombLabelX[i], CombLabelY[i], width: combinationLabelWidth);
            }
            potLabel = CreateLabel(PotLabelX, PotLabelY, PotWidth);
            betLabel = CreateLabel(BetLabelX, BetLabelY, BetWidth);
            testLabel = CreateLabel(TestLabelX, TestLabelY, TestLabelWidth, TestLabelHeight, TestLabelFontSize);
        }
        private void ShowDepositsLabels()
        {
            for (int i = 0; i < gameManager.GetPlayers().Count; i++)
            {
                IPlayer player = gameManager.GetPlayers()[i];
                depositLabels[i].Text = $"{player.GetName()}: {gameManager.GetPlayerDeposit(player)}";
                depositLabels[i].Visible = true;
            }
        }
        private void ShowPotLabel()
        {
            potLabel.Text = $"Pot: {gameManager.GetPot()}";
            potLabel.Visible = true;
        }
        private void ShowBetLabel()
        {
            betLabel.Text = $"Current Bet: {gameManager.GetBet()}";
            betLabel.Visible = true;
        }
        private void HideBetLabel()
        {
            betLabel.Visible = false;
        }
        private void ShowTestLabel(string text)
        {
            testLabel.Text = text;
            testLabel.Visible = true;
        }
        private void ShowCombinationLabels(int playerIndex)
        {
            string combinationName = gameManager.GetPlayerCombinations()[playerIndex].GetCombinationName();
            combinationLabels[playerIndex].Text = combinationName;
            combinationLabels[playerIndex].Visible = true;
        }
        private void HideCombinationLabels()
        {
            for (int i = 0; i < gameManager.GetPlayersCount(); i++)
                combinationLabels[i].Visible = false;
        }



        // АНИМАЦИЯ ПОБЕДИТЕЛЯ
        private async void InitializeWebView2()
        {
            webView2 = new WebView2();
            webView2.Visible = false;
            webView2.Location = new Point(0, -10);
            webView2.Size = new Size(WindowWidth, WindowHeight-15);
            webView2.DefaultBackgroundColor = Color.Transparent;
            await webView2.EnsureCoreWebView2Async(null);
            Controls.Add(webView2);
        }
        private async Task ShowVictoryAnimation()
        {
            IPlayer winner = gameManager.GetWinner();
            /*IPlayer winner = gameManager.GetPlayers()[4];*/
            string videoPath = winner.GetVictoryVideoPath();
            string audioPath = winner.GetVictoryAudioPath();

            string cirrentDir = Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location, @".."));
            string absIndexPath = Path.Combine(cirrentDir, indexPath);
            string absVideoPath = Path.Combine(cirrentDir, videoPath);
            string absoluteIndexPath = new Uri(absIndexPath).AbsoluteUri;
            string absoluteVideoPath = new Uri(absVideoPath).AbsoluteUri;
            UpdateIndexFile(absoluteVideoPath, absIndexPath);

            /*await webView2.EnsureCoreWebView2Async(null);*/
            webView2.CoreWebView2.Navigate(absoluteIndexPath);
            webView2.BringToFront();
            stopAnimationButton.BringToFront();
            await Task.Delay(1000);

            webView2.Visible = true;
            stopAnimationButton.Visible = true;
            
            audioFile = new AudioFileReader(audioPath);
            audioPlayer.Init(audioFile); 
            audioPlayer.Play();
            while (audioPlayer.PlaybackState == PlaybackState.Playing)
                await Task.Delay(100);

            webView2.Visible = false;
            stopAnimationButton.Visible = false;
        }



        public async void UpdateUI(IGameState state, string update, int playerIndex, string action)
        {
            if (update == "")
            {
                if (state is PreflopState)
                {
                    HidePlayersCards();
                    HideCommunityCards();
                    HideActionIcons();
                    HideCombinationLabels();
                    ShowPlayerCards(0);
                }

                else if (state is FlopState)
                    for (int i = 0; i < 3; i++)
                        ShowCommunityCard(i);

                else if (state is TurnState)
                    ShowCommunityCard(3);

                else if (state is RiverState)
                    ShowCommunityCard(4);

                else if (state is ShowdownState)
                {
                    await Task.Delay(1000);
                    for (int i = 0; i < gameManager.GetPlayersCount(); i++)
                    {
                        IPlayer player = gameManager.GetPlayers()[i];
                        if (gameManager.GetPlayerActive(player))
                        {
                            ShowPlayerCards(i);
                            ShowCombinationLabels(i);
                        }
                    }
                    await Task.Delay(1500);
                    ShowDepositsLabels();
                    ShowPotLabel();
                    await ShowVictoryAnimation();
                    ShowStartButton();
                }
            }

            else if (update == "Player Made Move")
            {
                switch (action)
                {
                    case "Bet": ShowActionIcon(playerIndex, BetIconPath); break;
                    case "Call": ShowActionIcon(playerIndex, CallIconPath); break;
                    case "Fold": ShowActionIcon(playerIndex, FoldIconPath); break;
                }
                ShowDepositsLabels();
                ShowPotLabel();
                await Task.Delay(500);
                if (action != "Fold")
                    HideActionIcon(playerIndex);
            }

            else if (update == "Awaiting For User Move")
            {
                ShowActionButtons();
                ShowBetLabel();
            }
        }
    }
}
