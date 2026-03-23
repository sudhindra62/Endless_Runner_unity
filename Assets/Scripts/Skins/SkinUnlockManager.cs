using UnityEngine;


    public class SkinUnlockManager : MonoBehaviour
    {
        private SkinsManager _skinsManager;
        private SkinManager _skinManager;

        private void Start()
        {
            _skinsManager = ServiceLocator.Get<SkinsManager>();
            _skinManager = SkinManager.Instance;
        }

        public bool IsSkinUnlocked(SkinData skin)
        {
            // In a real implementation, this would check against player data
            if (skin == null) return false;
            if (_skinsManager != null) return _skinsManager.IsSkinUnlocked(skin.skinID);
            return _skinManager != null && _skinManager.IsSkinUnlocked(skin.skinID);
        }

        public void UnlockSkin(SkinData skin, UnlockType unlockType)
        {
            if (!IsSkinUnlocked(skin))
            {
                if (_skinsManager != null) _skinsManager.UnlockSkin(skin.skinID);
                else if (_skinManager != null) _skinManager.UnlockSkin(skin.skinID);
                Debug.Log($"Skin {skin.skinName} unlocked via {unlockType.GetFormattedName()}!");
            }
        }
    }

