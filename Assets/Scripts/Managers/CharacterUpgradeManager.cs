using UnityEngine;
using System.Collections.Generic;

public class CharacterUpgradeManager : Singleton<CharacterUpgradeManager>
{
    [SerializeField] private List<CharacterData> allCharacters = new List<CharacterData>();
    private const string SelectedCharacterKey = "SelectedCharacterId";
    
    protected override void Awake()
    {
        base.Awake();
        LoadAllCharacterLevels();
    }

    public void UpgradeCharacter(CharacterData character)
    {
        if (character.currentUpgradeLevel >= character.maxUpgradeLevel) return;

        // Assuming a simple cost for now. This can be expanded.
        int upgradeCost = 100 * (character.currentUpgradeLevel + 1);

        if (ServiceLocator.Get<CurrencyManager>().SpendCoins(upgradeCost))
        {
            character.currentUpgradeLevel++;
            SaveCharacterLevel(character);
            Debug.Log($"Upgraded {character.displayName} to level {character.currentUpgradeLevel}");
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        if(character == null) return;
        PlayerPrefs.SetString(SelectedCharacterKey, character.characterId);
        ServiceLocator.Get<CharacterPassiveManager>()?.SetCharacter(character);
    }

    public CharacterData GetSelectedCharacter()
    {
        if (allCharacters.Count == 0) return null;

        string selectedId = PlayerPrefs.GetString(SelectedCharacterKey, allCharacters[0].characterId);
        return allCharacters.Find(c => c.characterId == selectedId);
    }

    private void SaveCharacterLevel(CharacterData character)
    {
        PlayerPrefs.SetInt(GetSaveKeyForCharacter(character), character.currentUpgradeLevel);
        PlayerPrefs.Save();
    }

    private void LoadAllCharacterLevels()
    {
        foreach (var character in allCharacters)
        {
            character.currentUpgradeLevel = PlayerPrefs.GetInt(GetSaveKeyForCharacter(character), 0);
        }
    }

    private string GetSaveKeyForCharacter(CharacterData character)
    {
        return "CharacterLevel_" + character.characterId;
    }
}
