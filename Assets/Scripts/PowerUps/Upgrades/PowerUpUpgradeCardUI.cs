
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpUpgradeCardUI : MonoBehaviour
{
    [SerializeField] private Image powerUpIcon;
    [SerializeField] private TextMeshProUGUI powerUpNameText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI upgradePreviewText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private Button upgradeButton;

    private PowerUpUpgradeData upgradeData;

    public void Setup(PowerUpUpgradeData data)
    {
        upgradeData = data;
        powerUpIcon.sprite = data.icon;
        powerUpNameText.text = data.powerUpName;

        int currentLevel = PowerUpUpgradeManager.Instance.GetPowerUpLevel(data.powerUpType);
        currentLevelText.text = "Level: " + currentLevel;

        if (currentLevel < data.upgradeTiers.Count)
        {
            PowerUpUpgradeTier nextTier = data.upgradeTiers[currentLevel];
            float currentValue = PowerUpUpgradeManager.Instance.GetPowerUpValue(data.powerUpType);
            upgradePreviewText.text = $"Next: {nextTier.value}";
            upgradeCostText.text = $"{nextTier.cost} {nextTier.currencyType}";
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }
        else
        {
            upgradePreviewText.text = "Max Level";
            upgradeCostText.text = "";
            upgradeButton.interactable = false;
        }
    }

    private void OnUpgradeClicked()
    {
        PowerUpUpgradeManager.Instance.UpgradePowerUp(upgradeData.powerUpType);
        // Refresh the UI after upgrade attempt
        Setup(upgradeData);
    }
}
