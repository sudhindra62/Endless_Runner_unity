
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Character
{
    public class CharacterSkinManager : MonoBehaviour
    {
        public static CharacterSkinManager Instance;

        public List<CharacterSkin> allSkins;
        private const string SelectedSkinKey = "SelectedCharacterSkin";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetSelectedSkin(CharacterSkin skin)
        {
            PlayerPrefs.SetString(SelectedSkinKey, skin.characterName);
            PlayerPrefs.Save();
        }

        public CharacterSkin GetSelectedSkin()
        {
            string skinName = PlayerPrefs.GetString(SelectedSkinKey, allSkins[0].characterName); // Default to the first skin
            return allSkins.Find(s => s.characterName == skinName);
        }

        public bool IsSkinUnlocked(CharacterSkin skin)
        {
            if (skin.unlockType == SkinUnlockType.Coins && skin.price == 0) return true; // Free skin
            return PlayerPrefs.GetInt(skin.characterName + "_unlocked", 0) == 1;
        }

        public bool UnlockSkin(CharacterSkin skin)
        {
            if (IsSkinUnlocked(skin)) return true;

            switch (skin.unlockType)
            {
                case SkinUnlockType.Coins:
                    if (CurrencyManager.Instance.SpendCoins(skin.price))
                    {
                        MarkSkinAsUnlocked(skin);
                        return true;
                    }
                    return false;

                case SkinUnlockType.Gems:
                    if (CurrencyManager.Instance.SpendGems(skin.price))
                    {
                        MarkSkinAsUnlocked(skin);
                        return true;
                    }
                    return false;

                case SkinUnlockType.Premium:
                    if (SubscriptionManager.Instance.IsSubscribed())
                    {
                        MarkSkinAsUnlocked(skin);
                        return true;
                    }
                    return false;
            }
            return false;
        }

        private void MarkSkinAsUnlocked(CharacterSkin skin)
        {
            PlayerPrefs.SetInt(skin.characterName + "_unlocked", 1);
            PlayerPrefs.Save();
        }
    }
}
