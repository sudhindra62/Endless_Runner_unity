using UnityEngine;


    public class SkinPurchaseManager : MonoBehaviour
    {
        private CurrencyManager _currencyManager;
        private SkinUnlockManager _skinUnlockManager;

        private void Start()
        {
            _currencyManager = ServiceLocator.Get<CurrencyManager>();
            _skinUnlockManager = ServiceLocator.Get<SkinUnlockManager>();
        }

        public void PurchaseSkin(SkinData skin)
        {
            if (skin == null || _skinUnlockManager == null || _currencyManager == null) return;

            if (_skinUnlockManager.IsSkinUnlocked(skin))
            {
                Debug.Log($"Attempted to purchase already unlocked skin: {skin.skinName}");
                return;
            }

            if (_currencyManager.SpendGems(skin.price))
            {
                _skinUnlockManager.UnlockSkin(skin, UnlockType.Purchased);
            }
            else
            {
                Debug.Log($"Purchase failed for skin: {skin.skinName}. Not enough gems.");
            }
        }
    }

