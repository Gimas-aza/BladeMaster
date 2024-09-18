using System.Collections.Generic;
using Assets.GameProgression;
using Assets.ShopManagement;
using Assets.GameSettings;

namespace Assets.DataStorageSystem
{
    [System.Serializable]
    public class DataStorage : IPlayerProgressionData, IShopData, ISettingsData
    {
        public int Money { get; set; } = 0;
        public int BestScore { get; set; } = 0;
        public int FinishedLevels { get; set; } = 0;
        public int UnlockedLevels { get; set; } = 1;
        public List<int> RatingScoreOfLevels { get; set; }
        public List<ItemData> Items { get; set; }
        public int QualityIndex { get; set; } = 0;
        public float Volume { get; set; } = 0.5f;
    }
}
