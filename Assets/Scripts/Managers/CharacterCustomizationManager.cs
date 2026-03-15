
using System;
using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class CharacterCustomizationManager : Singleton<CharacterCustomizationManager>
    {
        public List<Skin> availableSkins = new List<Skin>();
        public int currentSkinIndex = 0;

        public event Action<Skin> OnSkinChanged;

        private const string UnlockedSkinsKey = "UnlockedSkins";
        private const string CurrentSkinKey = "CurrentSkin";

        protected override void Awake()
        {
            base.Awake();
            LoadUnlockedSkins();
            LoadCurrentSkin();
        }

        public void UnlockSkin(int skinIndex)
        {
            if (skinIndex >= 0 && skinIndex < availableSkins.Count)
            {
                availableSkins[skinIndex].isUnlocked = true;
                SaveUnlockedSkins();
            }
        }

        public void SetCurrentSkin(int skinIndex)
        {
            if (availableSkins[skinIndex].isUnlocked)
            {
                currentSkinIndex = skinIndex;
                SaveCurrentSkin();
                OnSkinChanged?.Invoke(availableSkins[currentSkinIndex]);
            }
        }

        private void LoadUnlockedSkins()
        {
            string unlockedSkinsString = PlayerPrefs.GetString(UnlockedSkinsKey);
            if (!string.IsNullOrEmpty(unlockedSkinsString))
            {
                string[] unlockedIndices = unlockedSkinsString.Split(',');
                foreach (string indexString in unlockedIndices)
                {
                    if (int.TryParse(indexString, out int index))
                    {
                        if (index >= 0 && index < availableSkins.Count)
                        {
                            availableSkins[index].isUnlocked = true;
                        }
                    }
                }
            }
        }

        private void SaveUnlockedSkins()
        {
            List<string> unlockedIndices = new List<string>();
            for (int i = 0; i < availableSkins.Count; i++)
            {
                if (availableSkins[i].isUnlocked)
                {
                    unlockedIndices.Add(i.ToString());
                }
            }
            PlayerPrefs.SetString(UnlockedSkinsKey, string.Join(",", unlockedIndices));
            PlayerPrefs.Save();
        }

        private void LoadCurrentSkin()
        {
            currentSkinIndex = PlayerPrefs.GetInt(CurrentSkinKey, 0);
        }

        private void SaveCurrentSkin()
        {
            PlayerPrefs.SetInt(CurrentSkinKey, currentSkinIndex);
            PlayerPrefs.Save();
        }

        public Skin GetCurrentSkin()
        {
            return availableSkins[currentSkinIndex];
        }
    }

    [System.Serializable]
    public class Skin
    {
        public string skinName;
        public Material skinMaterial;
        public bool isUnlocked;
    }
}
