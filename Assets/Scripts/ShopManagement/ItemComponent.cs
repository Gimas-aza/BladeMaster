using System;
using Assets.GameProgression.Interfaces;
using UnityEngine;

namespace Assets.ShopManagement
{
    [CreateAssetMenu(fileName = "Item", menuName = "ShopManagement/Item")]
    public class ItemComponent : ScriptableObject, IItem, IItemSkin
    {
        [SerializeField] private GameObject _skin;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _icon;
        [SerializeField] private bool _isBought;
        [SerializeField] private bool _isEquipped;

        private ItemData _data;

        public int Price => _price;
        public Sprite Icon => _icon;
        public bool IsBought => _isBought;
        public bool IsEquipped => _isEquipped;

        public void Init(ItemData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            SyncWithData();
        }

        public GameObject GetSkin() => _skin;

        public void SetBought(bool isBought)
        {
            _isBought = isBought;
            _data.IsBought = isBought;
        }

        public void SetEquipped(bool isEquipped)
        {
            _isEquipped = isEquipped;
            _data.IsEquipped = isEquipped;
        }

        private void SyncWithData()
        {
            _isBought = _data.IsBought;
            _isEquipped = _data.IsEquipped;
        }
    }
}