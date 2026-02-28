using UnityEngine;

namespace Skins
{
    [CreateAssetMenu(fileName = "NewSkin", menuName = "Skins/Skin Data")]
    public class SkinData : ScriptableObject
    {
        public string skinName;
        public int price;
        public SkinRarity rarity;
    }
}
