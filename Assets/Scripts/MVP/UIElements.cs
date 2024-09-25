using System;
using System.Collections.Generic;
using Assets.MVP.State;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class UIElements
    {
        public Dictionary<Type, VisualElement> Menus;
        public VisualElement ContainerButtonsStartLevel;
        public VisualElement ViewItem;
        public VisualElement ContainerItemsShop;
        public VisualElement SettingsMenu;
        public VisualElement GameMenu;
        public List<VisualElement> GameStatistic;
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
        public Dictionary<Type, Button> ButtonsSettings;
        public Button ButtonExit;
        public Dictionary<Type, Button> ButtonsBackMainMenu;
        public Dictionary<Type, Button> ButtonsBack;
        public List<Button> ButtonsStartLevel;
        public Button ButtonBuyItem;
        public Button CurrentButtonItem;
        public Button ButtonPause;
        public Button ButtonContinue;
        public Button ButtonAgain;

        public DropdownField DropdownQuality;
        public Slider SliderVolume;

        public Action PreventButtonBuyItem;

        public UIElements(VisualElement root)
        {
            Menus = new Dictionary<Type, VisualElement>
            {
                { typeof(StateMainMenu), root.Q<VisualElement>("MainMenu") },
                { typeof(StateLevelsMenu), root.Q<VisualElement>("LevelsMenu") },
                { typeof(StateShopMenu), root.Q<VisualElement>("ShopMenu") },
                { typeof(StateSettingsMenu), root.Q<VisualElement>("SettingsMenu") },
                { typeof(StatePauseMenu), root.Q<VisualElement>("PauseMenu") },
                { typeof(StateFinishedMenu), root.Q<VisualElement>("FinishedLevelMenu") }
            };
            ButtonsBack = new Dictionary<Type, Button>
            {
                { typeof(StateLevelsMenu), Menus[typeof(StateLevelsMenu)].Q<Button>("ButtonBack") },
                { typeof(StateShopMenu), Menus[typeof(StateShopMenu)].Q<Button>("ButtonBack") },
                { typeof(StateSettingsMenu), Menus[typeof(StateSettingsMenu)].Q<Button>("ButtonBack") }
            };
            ButtonsSettings = new Dictionary<Type, Button>
            {
                { typeof(StateMainMenu), Menus[typeof(StateMainMenu)].Q<Button>("ButtonSettings") },
                { typeof(StatePauseMenu), Menus[typeof(StatePauseMenu)].Q<Button>("ButtonSettings") },
                { typeof(StateFinishedMenu), Menus[typeof(StateFinishedMenu)].Q<Button>("ButtonSettings") }
            };
            ButtonsBackMainMenu = new Dictionary<Type, Button>
            {
                { typeof(StatePauseMenu), Menus[typeof(StatePauseMenu)].Q<Button>("ButtonBackMainMenu") },
                { typeof(StateFinishedMenu), Menus[typeof(StateFinishedMenu)].Q<Button>("ButtonBackMainMenu") }
            };

            ContainerButtonsStartLevel = root.Q<VisualElement>("ContainerButtonsStartLevel");
            ViewItem = root.Q<VisualElement>("ViewItem");
            ContainerItemsShop = root.Q<VisualElement>("GroupBoxItemShop");
            SettingsMenu = root.Q<VisualElement>("SettingsMenu");
            GameMenu = root.Q<VisualElement>("GameMenu");
            GameStatistic = root.Query<VisualElement>("Statistic").ToList();
            RatingScoreEmpty = Menus[typeof(StateFinishedMenu)].Query<VisualElement>("RatingScoreEmpty").ToList();
            RatingScoreFull = Menus[typeof(StateFinishedMenu)].Query<VisualElement>("RatingScoreFull").ToList();

            BestScore = root.Q<Label>("LabelBestScore");
            Counter = root.Query<Label>("Counter").ToList();
            Money = root.Query<Label>("Money").ToList();
            Win = root.Q<Label>("LabelWin");
            Lose = root.Q<Label>("LabelLose");
            AmountKnives = root.Q<Label>("LabelAmountKnives");
            PressureForce = root.Q<ProgressBar>("ProgressBarPressureForce");

            ButtonPlay = root.Q<Button>("ButtonPlay");
            ButtonShop = root.Q<Button>("ButtonShop");
            ButtonExit = root.Q<Button>("ButtonExit");
            ButtonsStartLevel = new List<Button>();
            ButtonBuyItem = root.Q<Button>("ButtonBuy");
            ButtonPause = root.Q<Button>("ButtonPause");
            ButtonContinue = root.Q<Button>("ButtonContinue");
            ButtonAgain = root.Q<Button>("ButtonAgain");

            DropdownQuality = root.Q<DropdownField>("DropdownQuality");
            SliderVolume = root.Q<Slider>("SliderVolume");
        }
    }
}