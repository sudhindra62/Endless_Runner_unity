using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages player character stat upgrades and progression.
/// Synchronized with PlayerDataManager and SaveManager.
/// </summary>
public class CharacterUpgradeManager : Singleton<CharacterUpgradeManager>
{
    [System.Serializable]
    public class StatUpgrade
    {
        public string statId;
        public int currentLevel;
        public int maxLevel = 10;
        public int baseCost = 1000;
        public float costMultiplier = 1.5f;
    }

    public List<StatUpgrade> statUpgrades = new List<StatUpgrade>();
    [SerializeField] private string selectedCharacter = "Default";
    [SerializeField] private List<CharacterData> availableCharacters = new List<CharacterData>();

    protected override void Awake()
    {
        base.Awake();
        InitializeStats();
    }

    private void InitializeStats()
    {
        // Default stats if none exist
        if (statUpgrades.Count == 0)
        {
            statUpgrades.Add(new StatUpgrade { statId = "Speed", currentLevel = 1 });
            statUpgrades.Add(new StatUpgrade { statId = "Jump", currentLevel = 1 });
            statUpgrades.Add(new StatUpgrade { statId = "Magnet", currentLevel = 1 });
        }
    }

    public int GetStatLevel(string statId)
    {
        var stat = statUpgrades.Find(s => s.statId == statId);
        return stat?.currentLevel ?? 1;
    }

    public bool UpgradeStat(string statId)
    {
        var stat = statUpgrades.Find(s => s.statId == statId);
        if (stat == null || stat.currentLevel >= stat.maxLevel) return false;

        int cost = Mathf.RoundToInt(stat.baseCost * Mathf.Pow(stat.costMultiplier, stat.currentLevel - 1));

        if (PlayerDataManager.Instance.SpendCurrency(CurrencyType.Coins, cost))
        {
            stat.currentLevel++;
            SaveManager.Instance.SaveGame();
            Debug.Log($"Guardian Architect: Upgraded {statId} to level {stat.currentLevel}.");
            return true;
        }

        return false;
    }

    public string GetSelectedCharacterID() => selectedCharacter;
    public CharacterData GetSelectedCharacterData()
    {
        CharacterData selected = availableCharacters.Find(character => character != null && character.characterId == selectedCharacter);
        if (selected != null)
        {
            return selected;
        }

        return availableCharacters.Count > 0 ? availableCharacters[0] : null;
    }

    public CharacterData GetSelectedCharacter() => GetSelectedCharacterData();

    public bool UpgradeCharacter(string characterId)
    {
        if (!string.IsNullOrEmpty(characterId))
        {
            selectedCharacter = characterId;
        }

        return UpgradeStat("Speed");
    }

    public bool UpgradeCharacter(CharacterData character)
    {
        if (character == null)
        {
            return false;
        }

        return UpgradeCharacter(character.characterId);
    }
}
