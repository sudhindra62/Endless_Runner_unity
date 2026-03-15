
using UnityEngine;

namespace EndlessRunner.Data
{
    [CreateAssetMenu(fileName = "ShopItem", menuName = "EndlessRunner/Shop Item", order = 0)]
    public class ShopItemData : ScriptableObject
    {
        public string Name;
        public string Description;
        public int Price;
        public ItemType Type;
    }
}
