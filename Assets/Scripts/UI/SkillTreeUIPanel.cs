using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkillTreeUIPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI availablePointsText;
    [SerializeField] private GameObject skillNodePrefab;
    [SerializeField] private Transform skillNodeContainer;

    // Could be loaded from a config or assigned in inspector
    [SerializeField] private List<SkillNodeData> skillNodesToShow;

    private void OnEnable()
    {
        // Subscribe to events
        if (SkillTreeManager.Instance != null)
        {
            SkillTreeManager.Instance.OnSkillTreeUpdated += UpdateUI;
        }
        
        // Initial setup
        CreateSkillTreeNodes();
        UpdateUI();
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (SkillTreeManager.Instance != null)
        {
            SkillTreeManager.Instance.OnSkillTreeUpdated -= UpdateUI;
        }
    }

    private void CreateSkillTreeNodes()
    {
        // Clear any existing nodes to prevent duplication on re-enable
        foreach (Transform child in skillNodeContainer)
        {
            Destroy(child.gameObject);
        }

        // Create a UI element for each skill
        foreach (var nodeData in skillNodesToShow)
        {
            GameObject nodeObject = Instantiate(skillNodePrefab, skillNodeContainer);
            var nodeUI = nodeObject.GetComponent<SkillNodeUI>(); // Assuming a SkillNodeUI component
            if (nodeUI != null)
            {
                nodeUI.Setup(nodeData);
            }
        }
    }

    private void UpdateUI()
    {
        if (SkillTreeManager.Instance == null) return;
        availablePointsText.text = $"Skill Points: {SkillTreeManager.Instance.AvailableSkillPoints}";
        
        // The individual nodes will update themselves via their own subscriptions
        // or we can manually trigger an update here if needed.
    }
}
