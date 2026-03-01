using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillNodeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI modifierText;
    [SerializeField] private Button upgradeButton;

    private SkillNodeData nodeData;

    public void Setup(SkillNodeData data)
    {
        nodeData = data;
        UpdateNodeVisuals();
    }

    private void OnEnable()
    {
        // Subscribe to get updates when any skill is upgraded
        if (SkillTreeManager.Instance != null)
        {
            SkillTreeManager.Instance.OnSkillTreeUpdated += UpdateNodeVisuals;
        }
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    private void OnDisable()
    {
        if (SkillTreeManager.Instance != null)
        {
            SkillTreeManager.Instance.OnSkillTreeUpdated -= UpdateNodeVisuals;
        }
        upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
    }

    private void UpdateNodeVisuals()
    {
        if (nodeData == null) return;

        nameText.text = nodeData.displayName;
        levelText.text = $"Lvl: {nodeData.currentLevel}/{nodeData.maxLevel}";
        modifierText.text = GetModifierDescription();

        // Update button state
        bool canUpgrade = SkillTreeManager.Instance.AvailableSkillPoints > 0 && nodeData.currentLevel < nodeData.maxLevel;
        upgradeButton.interactable = canUpgrade;
    }

    private string GetModifierDescription()
    {
        float nextValue = nodeData.baseModifierValue + (nodeData.incrementPerLevel * nodeData.currentLevel);
        string description = nodeData.modifierType.ToString(); // Basic description
        // This could be a more complex mapping to a user-friendly string
        return $"+{nextValue} to {description}"; 
    }

    private void OnUpgradeClicked()
    {
        if (SkillTreeManager.Instance != null)
        {
            SkillTreeManager.Instance.UpgradeSkill(nodeData);
            // The OnSkillTreeUpdated event will trigger the UI update
        }
    }
}
