using UnityEngine;
using Systems;

namespace Skins
{
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
            if (_skinUnlockManager.IsSkinUnlocked(skin))
            {
                Debug.Log($"Attempted to purchase already unlocked skin: {skin.skinName}");
                return;
            }

            if (_currencyManager.TrySpendGems(skin.price))
            {
                _skinUnlockManager.UnlockSkin(skin, UnlockType.Purchased);
            }
            else
            {
                Debug.Log($"Purchase failed for skin: {skin.skinName}. Not enough gems.");
            }
        }
    }
}
