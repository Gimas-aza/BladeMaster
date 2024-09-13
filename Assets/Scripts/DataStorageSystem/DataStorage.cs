using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.GameProgression;
using Assets.Knife;
using Assets.ShopManagement;
using UnityEngine;

namespace Assets.DataStorageSystem
{
    [System.Serializable]
    public class DataStorage : IPlayerProgressionData, IShopData
    {
        public int Money { get; set; } = 0;
        public int BestScore { get; set; } = 0;
        public int FinishedLevels { get; set; } = 0;
        public int UnlockedLevels { get; set; } = 1;
        public List<ItemData> Items { get; set; }
    }
}
