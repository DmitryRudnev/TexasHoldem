using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TexasHoldem
{
    public class UILayoutConfig
    {
        // Пути
        public const string TableFolder = "Media/Table Images";
        public const string TableImagePath = TableFolder + "/table3.png";
        public const string PotImagePath = TableFolder + "/pot.png";
        public const string IconImagePath = TableFolder + "/poker.ico";

        public const string CardImagesFolder = "Media/Card Images";
        public const string PlayerAvatarsFolder = "Media/Player Avatars";
        
        public const string VictoryAnimationsFolder = "Media/Winner Animations";
        public const string VictoryAnimationGifsFolder = VictoryAnimationsFolder + "/Gifs";
        public const string VictoryAnimationAudiosFolder = VictoryAnimationsFolder + "/Audios";
        public const string VictoryAnimationVideosFolder = VictoryAnimationsFolder + "/Videos";

        public const string ActionIconsFolder = "Media/Action Icons"; 
        public const string BetIconPath = ActionIconsFolder + "/bet.png";
        public const string CallIconPath = ActionIconsFolder + "/call.png";
        public const string FoldIconPath = ActionIconsFolder + "/fold.png";



        // Общее
        public const int MaxPlayerNumber = 6; 
        public const int WindowWidth = 1550;  // 1920
        public const int WindowHeight = 840;  // 1080
        public const int DefaultWidth = 140;  // 140
        public const int DefaultHeight = 30;
        public const int DefaultFontSize = 14;



        // Карты
        public const string CardBackPath = CardImagesFolder + "/back.png";
        public const int CardWidth = 60;
        public const int CardHeight = 90;
        public const int CardSpacing = CardWidth + 10;

        // Личные карты
        public static int[] PlayerCardsY = new int[MaxPlayerNumber] { 490, 330, 220, 220, 220, 330 };
        public static int[] PlayerCard1X = new int[MaxPlayerNumber] { 700, 240, 430, 690, 980, 1150};
        public static int[] PlayerCard2X = GetPlayerCard2X();
        public static int[] GetPlayerCard2X()
        {
            int[] temp = new int[MaxPlayerNumber];
            for (int i = 0; i < MaxPlayerNumber; i++) 
                temp[i] = PlayerCard1X[i] + CardSpacing;
            return temp;
        }

        // Общие карты
        public const int CommunityCardY = 380;
        public const int CommunityCardStartX = 595;
        public static int[] CommunityCardX = GetCommunityCardX();
        public static int[] GetCommunityCardX()
        {
            int[] temp = new int[MaxPlayerNumber];
            for (int i = 0; i < MaxPlayerNumber; i++) 
                temp[i] = CommunityCardStartX + CardSpacing * i;
            return temp;
        }



        // Кнопки
        public const int StartButtonWidth = 120;
        public const int StartButtonHeihgt = DefaultHeight;
        public const int StartButtonX = 530;
        public const int StartButtonY = 597;

        public const int ActionButtonWidth = 100;
        public const int ActionButtonHeight = DefaultHeight;
        public const int ActionButtonY = 598;
        public const int CallButtonX = 855;
        public const int FoldButtonX = CallButtonX + ActionButtonWidth + 5;



        // Метка общего приза
        public const int PotLabelX = 440;
        public const int PotLabelY = 480;
        public const int PotWidth = DefaultWidth - 50;
        // Метка текущей ставки
        public const int BetLabelX = CallButtonX + 35;
        public const int BetLabelY = ActionButtonY - 35;
        public const int BetWidth = DefaultWidth;
        // Метки депозитов игроков
        public static int[] PlayerLabelY = new int[MaxPlayerNumber]
        {
            PlayerCardsY[0] + 110,
            PlayerCardsY[1] + 10,
            PlayerCardsY[2] - 45,
            PlayerCardsY[3] - 45,
            PlayerCardsY[4] - 45,
            PlayerCardsY[5] + 15
        };
        public static int[] PlayerLabelX = new int[MaxPlayerNumber] 
        {
            PlayerCard1X[0] + 5,
            PlayerCard1X[1] - 190,
            PlayerCard1X[2] + 15,
            PlayerCard1X[3] + 3,
            PlayerCard1X[4] + 20,
            PlayerCard1X[5] + 220
        };
        // Метки комбинаций
        public const int combinationLabelWidth = DefaultWidth + 50;
        public static int[] CombLabelX = GetCombLabelX();
        public static int[] CombLabelY = GetCombLabelY(); 
        public static int[] GetCombLabelX()
        {
            int[] temp = new int[MaxPlayerNumber];
            temp[0] = PlayerCard1X[0] + CardWidth * 2 + 20;
            for (int i = 1; i < MaxPlayerNumber; i++)
                temp[i] = PlayerCard1X[i];
            return temp;
        }
        public static int[] GetCombLabelY()
        {
            int[] temp = new int[MaxPlayerNumber];
            temp[0] = PlayerCardsY[0] + CardHeight - DefaultHeight + 10;
            for (int i = 1; i < MaxPlayerNumber; i++)
                temp[i] = PlayerCardsY[i] + CardHeight + 10;
            return temp;
        }
        // Тестовая метка
        public const int TestLabelX = 10;
        public const int TestLabelY = 10;
        public const int TestLabelWidth = 30;
        public const int TestLabelHeight = 15;
        public const int TestLabelFontSize = 10;



        // Иконки
        public const int IconTimeout = 1000;
        public const int IconWidth = 50;
        public const int IconHeight = 50;
        public static int[] IconX = GetIconX();
        public static int[] IconY = GetIconY(); 
        public static int[] GetIconX()
        {
            int[] temp = new int[MaxPlayerNumber];
            for (int i = 1; i < MaxPlayerNumber; i++)
                temp[i] = PlayerLabelX[i] + DefaultWidth - 30;
            temp[0] = PlayerLabelX[0] + DefaultWidth - 10;
            temp[3] = PlayerLabelX[3] + DefaultWidth - 25;
            temp[5] = PlayerLabelX[5] + DefaultWidth - 80;
            return temp;
        }
        public static int[] GetIconY()
        {
            int[] temp = new int[MaxPlayerNumber];
            temp[0] = PlayerLabelY[0] + 50;
            for (int i = 1; i < MaxPlayerNumber; i++)
                temp[i] = PlayerLabelY[i] - 170;
            temp[1] = PlayerLabelY[1] - 190;
            temp[3] = PlayerLabelY[3] - 175;
            temp[5] = PlayerLabelY[5] - 200;
            return temp;
        }



        // Winner Animation
        public const string indexPath = "Media/Winner Animations/Videos/index.html";
        public static void UpdateIndexFile(string absoluteVideoPath, string absoluteIndexPath)
        {
            string htmlContent = $@"
            <html>
            <body style=""background: transparent;"">
                <video autoplay muted style=""width: 100%; height: 100%;"">
                    <source src=""{absoluteVideoPath}"" type=""video/webm"">
                </video>
            </body>
            </html>";

            File.WriteAllText(absoluteIndexPath, htmlContent);
        }
    }
}