using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUIPanel : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI passiveDescriptionText;
    [SerializeField] private TextMeshProUGUI upgradeLevelText;
    [SerializeField] private Button upgradeButton;

    private CharacterData currentCharacter;

    private void OnEnable()
    {
        // This would be triggered by a character selection event
        // For now, we'll just display the selected character
        currentCharacter = CharacterUpgradeManager.Instance.GetSelectedCharacterData();
        UpdateUI();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }

    private void OnDisable()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
    }

    private void UpdateUI()
    {
        if (currentCharacter == null) return;
        characterNameText.text = currentCharacter.displayName;
        passiveDescriptionText.text = GetPassiveDescription(currentCharacter);
        upgradeLevelText.text = $"Level {currentCharacter.currentUpgradeLevel}/{currentCharacter.maxUpgradeLevel}";
        upgradeButton.interactable = currentCharacter.currentUpgradeLevel < currentCharacter.maxUpgradeLevel;
    }

    private void OnUpgradeButtonClicked()
    {
        CharacterUpgradeManager.Instance.UpgradeCharacter(currentCharacter);
        UpdateUI();
    }

    private string GetPassiveDescription(CharacterData character)
    {
        string description = "";
        switch (character.passiveType)
        {
            case PassiveType.MagnetDurationBoost:
                description = $"Increases magnet duration by {character.GetCurrentPassiveValue() * 100}%";
                break;
            case PassiveType.CoinValueBoost:
                description = $"Increases coin value by {character.GetCurrentPassiveValue()}";
                break;
            case PassiveType.ExtraRevive:
                description = $"Grants {character.GetCurrentPassiveValue()} extra revive(s)";
                break;
            case PassiveType.DifficultyReduction:
                description = $"Reduces game difficulty by {character.GetCurrentPassiveValue() * 100}%";
                break;
        }
        return description;
    }
}
