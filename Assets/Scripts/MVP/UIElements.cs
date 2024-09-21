using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class UIElements
    {
        public Dictionary<StateMenu, VisualElement> Menus;
        public VisualElement ContainerButtonsStartLevel;
        public VisualElement ViewItem;
        public VisualElement ContainerItemsShop;
        public VisualElement SettingsMenu;
        public VisualElement GameMenu;
        public VisualElement GameMenuInfo;
        public List<VisualElement> GameStatistic;
        public VisualElement PauseMenu;
        public VisualElement FinishedLevelMenu;
        public List<VisualElement> RatingScoreEmpty;
        public List<VisualElement> RatingScoreFull;

        public List<Label> Counter;
        public List<Label> Money;
        public Label Win;
        public Label Lose;
        public Label AmountKnives;
        public ProgressBar PressureForce;
        public Label BestScore;

        public Button ButtonPlay;
        public Button ButtonShop;
        public Button ButtonSettings;
        public Button ButtonExit;
        public List<Button> ButtonBack;
        public List<Button> ButtonsStartLevel;
        public Button ButtonBuyItem;
        public Button CurrentButtonItem;
        public Button ButtonPause;
        public Button ButtonContinue;
        public List<Button> ButtonsSettings;
        public List<Button> ButtonsBackMainMenu;
        public Button ButtonAgain;
        public Button ButtonBackGameMenu;

        public DropdownField DropdownQuality;
        public Slider SliderVolume;

        public Action PreventButtonBuyItem;

        public UIElements(VisualElement root)
        {
            Menus = new Dictionary<StateMenu, VisualElement>
            {
                { StateMenu.MainMenu, root.Q<VisualElement>("MainMenu") },
                { StateMenu.LevelsMenu, root.Q<VisualElement>("LevelsMenu") },
                { StateMenu.ShopMenu, root.Q<VisualElement>("ShopMenu") },
                { StateMenu.SettingsMenu, root.Q<VisualElement>("SettingsMenu") }
            };

            ContainerButtonsStartLevel = root.Q<VisualElement>("ContainerButtonsStartLevel");
            ViewItem = root.Q<VisualElement>("ViewItem");
            ContainerItemsShop = root.Q<VisualElement>("GroupBoxItemShop");
            SettingsMenu = root.Q<VisualElement>("SettingsMenu");
            GameMenu = root.Q<VisualElement>("GameMenu");
            GameMenuInfo = root.Q<VisualElement>("GameMenu-Info");
            GameStatistic = root.Query<VisualElement>("Statistic").ToList();
            PauseMenu = root.Q<VisualElement>("PauseMenu");
            FinishedLevelMenu = root.Q<VisualElement>("FinishedLevelMenu");
            RatingScoreEmpty = FinishedLevelMenu.Query<VisualElement>("RatingScoreEmpty").ToList();
            RatingScoreFull = FinishedLevelMenu.Query<VisualElement>("RatingScoreFull").ToList();

            BestScore = root.Q<Label>("LabelBestScore");
            Counter = root.Query<Label>("Counter").ToList();
            Money = root.Query<Label>("Money").ToList();
            Win = root.Q<Label>("LabelWin");
            Lose = root.Q<Label>("LabelLose");
            AmountKnives = root.Q<Label>("LabelAmountKnives");
            PressureForce = root.Q<ProgressBar>("ProgressBarPressureForce");

            ButtonPlay = root.Q<Button>("ButtonPlay");
            ButtonShop = root.Q<Button>("ButtonShop");
            ButtonSettings = root.Q<Button>("ButtonSettings");
            ButtonExit = root.Q<Button>("ButtonExit");
            ButtonBack = root.Query<Button>("ButtonBack").ToList();
            ButtonsStartLevel = new List<Button>();
            ButtonBuyItem = root.Q<Button>("ButtonBuy");
            ButtonPause = root.Q<Button>("ButtonPause");
            ButtonContinue = root.Q<Button>("ButtonContinue");
            ButtonsSettings = root.Query<Button>("ButtonSettings").ToList();
            ButtonsBackMainMenu = root.Query<Button>("ButtonBackMainMenu").ToList();
            ButtonAgain = root.Q<Button>("ButtonAgain");
            ButtonBackGameMenu = SettingsMenu.Q<Button>("ButtonBack");

            DropdownQuality = root.Q<DropdownField>("DropdownQuality");
            SliderVolume = root.Q<Slider>("SliderVolume");
        }
    }
}