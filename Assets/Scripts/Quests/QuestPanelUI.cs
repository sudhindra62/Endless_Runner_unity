
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the primary quest interface, including tabs for different quest types.
/// Populates the UI with QuestCardUI prefabs based on the active quests in QuestManager.
/// </summary>
public class QuestPanelUI : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform dailyTabContent;
    [SerializeField] private Transform weeklyTabContent;
    [SerializeField] private Transform eventTabContent;

    private void OnEnable()
    {
        // Subscribe to QuestManager events to keep the UI in sync
        QuestManager.OnQuestLogChanged += PopulateQuests;
        // Initial population of the UI
        PopulateQuests();
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        QuestManager.OnQuestLogChanged -= PopulateQuests;
    }

    private void PopulateQuests()
    {
        if (QuestManager.Instance == null) return;

        ClearTabs();

        // Use Quest directly since QuestManager stores Quest objects
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            // Simple placement — all in daily tab by default if no questType
            Transform parentTab = dailyTabContent;
            if (parentTab != null)
            {
                GameObject cardGO = Instantiate(cardPrefab, parentTab);
                QuestCardUI cardUI = cardGO.GetComponent<QuestCardUI>();
                if (cardUI != null)
                {
                    cardUI.SetQuestData(quest);
                }
            }
        }
    }

    private void ClearTabs()
    {
        foreach (Transform child in dailyTabContent) { Destroy(child.gameObject); }
        foreach (Transform child in weeklyTabContent) { Destroy(child.gameObject); }
        foreach (Transform child in eventTabContent) { Destroy(child.gameObject); }
    }

    private Transform GetParentTab(QuestType questType)
    {
        switch (questType)
        {
            case QuestType.Daily:
                return dailyTabContent;
            case QuestType.Weekly:
                return weeklyTabContent;
            case QuestType.Event:
                return eventTabContent;
            default:
                Debug.LogWarning($"Quest type {questType} does not have a designated UI tab.");
                return null;
        }
    }
}
