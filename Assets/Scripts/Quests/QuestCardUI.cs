
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the individual UI card for a single quest.
/// Displays quest details and handles player interaction for claiming and rerolling.
/// </summary>
public class QuestCardUI : MonoBehaviour
{
    public QuestProgressTracker questTracker { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI rewardText; // Shows Coins, Gems, XP
    [SerializeField] private Image rewardItemIcon; // Shows item like a chest
    [SerializeField] private Button claimButton;
    [SerializeField] private Button rerollButton;

    private QuestPanelUI ownerPanel;

    public void SetQuestData(QuestProgressTracker tracker, QuestPanelUI panel)
    {
        this.questTracker = tracker;
        this.ownerPanel = panel;
        
        // Remove any previous listeners to prevent stacking
        claimButton.onClick.RemoveAllListeners();
        rerollButton.onClick.RemoveAllListeners();

        // Add fresh listeners
        claimButton.onClick.AddListener(OnClaim);
        rerollButton.onClick.AddListener(OnReroll);
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (questTracker == null) return;

        QuestData data = questTracker.questData;
        descriptionText.text = data.description;
        progressBar.maxValue = data.requiredProgress;
        progressBar.value = questTracker.currentProgress;
        progressText.text = $"{questTracker.currentProgress} / {data.requiredProgress}";
        
        // Build reward string
        string rewards = "";
        if (data.rewardCoins > 0) rewards += $"Coins: {data.rewardCoins} ";
        if (data.rewardGems > 0) rewards += $"Gems: {data.rewardGems} ";
        if (data.rewardXP > 0) rewards += $"XP: {data.rewardXP}";
        rewardText.text = rewards.Trim();

        // Handle item icon
        if (rewardItemIcon != null)
        {
            if(data.rewardItemPrefab != null)
            {
                // This assumes you have a way to get a Sprite from a GameObject prefab, e.g., a component with the icon sprite.
                // var itemSprite = data.rewardItemPrefab.GetComponent<Image>().sprite; // Placeholder logic
                // rewardItemIcon.sprite = itemSprite;
                rewardItemIcon.gameObject.SetActive(true);
            }
            else
            {
                rewardItemIcon.gameObject.SetActive(false);
            }
        }

        // Control button visibility
        bool isCompleted = questTracker.isCompleted;
        claimButton.gameObject.SetActive(isCompleted);
        rerollButton.gameObject.SetActive(!isCompleted);
    }

    private void OnClaim()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ClaimReward(questTracker);
        }
        // The QuestPanelUI will handle destroying this object after the event fires.
    }

    private void OnReroll()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.RerollQuest(questTracker);
        }
    }
}
