using Assets.Knife;
using UnityEngine;

namespace Assets.ShopManagement
{
    public class ItemComponent : MonoBehaviour, IItem, IItemSkin
    {
        [SerializeField] private GameObject _skin;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _icon;

        public int Price => _price;
        public Sprite Icon => _icon;

        public bool IsBought { get; set; } = false;

        public GameObject GetSkin()
        {
            return _skin;
        }
    }
}