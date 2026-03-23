
using System.Collections.Generic;
using UnityEngine;

    public class CharacterSkinManager : MonoBehaviour
    {
        public static CharacterSkinManager Instance;

        public List<CharacterSkin> allSkins;
        private const string SelectedSkinKey = "SelectedCharacterSkin";

        public static System.Action<CharacterSkin> OnSkinChanged;

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
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.activeTheme = skin.characterName;
                SaveManager.Instance.SaveGame();
            }
        }

        public CharacterSkin GetSelectedSkin()
        {
            string skinName = (SaveManager.Instance != null && !string.IsNullOrEmpty(SaveManager.Instance.Data.activeTheme)) 
                ? SaveManager.Instance.Data.activeTheme 
                : allSkins[0].characterName;
            return allSkins.Find(s => s.characterName == skinName);
        }

        public bool IsSkinUnlocked(CharacterSkin skin) => IsSkinUnlocked(skin.characterName);
        
        public bool IsSkinUnlocked(string skinName)
        {
            var skin = allSkins.Find(s => s.characterName == skinName);
            if (skin != null && skin.unlockType == SkinUnlockType.Coins && skin.price == 0) return true;
            return SaveManager.Instance != null && SaveManager.Instance.Data.unlockedSkins.Contains(skinName);
        }

        public bool UnlockSkin(string skinName)
        {
            var skin = allSkins.Find(s => s.characterName == skinName);
            return skin != null && UnlockSkin(skin);
        }

        public bool UnlockSkin(CharacterSkin skin)
        {
            if (IsSkinUnlocked(skin)) return true;

            switch (skin.unlockType)
            {
                case SkinUnlockType.Coins:
                    if (PlayerDataManager.Instance.SpendCurrency(CurrencyType.Coins, skin.price))
                    {
                        MarkSkinAsUnlocked(skin);
                        return true;
                    }
                    return false;

                case SkinUnlockType.Gems:
                    if (PlayerDataManager.Instance.SpendCurrency(CurrencyType.Gems, skin.price))
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

        public void EquipSkin(string skinName)
        {
            var skin = allSkins.Find(s => s.characterName == skinName);
            if (skin != null && IsSkinUnlocked(skin))
            {
                SetSelectedSkin(skin);
                OnSkinChanged?.Invoke(skin);
            }
        }

        private void MarkSkinAsUnlocked(CharacterSkin skin)
        {
            if (SaveManager.Instance != null && !SaveManager.Instance.Data.unlockedSkins.Contains(skin.characterName))
            {
                SaveManager.Instance.Data.unlockedSkins.Add(skin.characterName);
                SaveManager.Instance.SaveGame();
            }
        }
    }

