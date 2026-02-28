using UnityEngine;
using Systems;

namespace Skins
{
    public class SkinUnlockManager : MonoBehaviour
    {
        private SkinsManager _skinsManager;

        private void Start()
        {
            _skinsManager = ServiceLocator.Get<SkinsManager>();
        }

        public bool IsSkinUnlocked(SkinData skin)
        {
            // In a real implementation, this would check against player data
            return _skinsManager.IsSkinUnlocked(skin.skinName);
        }

        public void UnlockSkin(SkinData skin, UnlockType unlockType)
        {
            if (!IsSkinUnlocked(skin))
            {
                _skinsManager.UnlockSkin(skin.skinName);
                Debug.Log($"Skin {skin.skinName} unlocked via {unlockType.GetFormattedName()}!");
            }
        }
    }
}
