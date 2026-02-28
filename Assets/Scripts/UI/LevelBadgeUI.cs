using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBadgeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image xpFillImage;

    private PlayerDataManager playerDataManager;

    private void Start()
    {
        playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        if (playerDataManager == null) 
        {
            Debug.LogError("PlayerDataManager not found.");
            return;
        }

        playerDataManager.OnLevelChanged += UpdateLevel;
        playerDataManager.OnXPChanged += UpdateXP;

        // Initial UI update
        UpdateLevel(playerDataManager.Level);
        UpdateXP(playerDataManager.XP, playerDataManager.XpForNextLevel);
    }

    private void OnDestroy()
    {
        if (playerDataManager == null) return;
        playerDataManager.OnLevelChanged -= UpdateLevel;
        playerDataManager.OnXPChanged -= UpdateXP;
    }

    private void UpdateLevel(int level)
    {
        if (levelText != null) 
        {
            levelText.text = level.ToString();
        }
    }

    private void UpdateXP(int currentXP, int xpForNextLevel)
    {
        if (xpFillImage != null)
        {
            xpFillImage.fillAmount = (float)currentXP / xpForNextLevel;
        } 
    }
}
