using Assets.EntryPoint;
using Assets.Knife;
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
            _data = data;
            _isBought = data.IsBought;
            _isEquipped = data.IsEquipped;
        }

        public GameObject GetSkin()
        {
            return _skin;
        }

        public void SetBought(bool isBought)
        {
            _data.IsBought = isBought;
            _isBought = isBought;
        }

        public void SetEquipped(bool isEquipped)
        {
            _data.IsEquipped = isEquipped;
            _isEquipped = isEquipped;
        }
    }
}