using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the UI for the milestone screen. It dynamically creates and updates milestone entries.
/// </summary>
public class MilestoneUI : MonoBehaviour
{
    [Header("UI Setup")]
    [Tooltip("The prefab for a single milestone entry in the list.")]
    [SerializeField] private GameObject milestoneEntryPrefab;
    [Tooltip("The container where milestone entries will be instantiated.")]
    [SerializeField] private Transform listContainer;

    // A dictionary to keep track of the UI instances for each milestone ID
    private Dictionary<string, GameObject> milestoneUIEntries = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        // Subscribe to events when the UI is active
        MilestoneManager.OnProgressUpdated += UpdateMilestoneEntry;
        MilestoneManager.OnMilestoneClaimed += HandleMilestoneClaimed;
    }

    private void OnDisable()
    {
        // Always unsubscribe
        MilestoneManager.OnProgressUpdated -= UpdateMilestoneEntry;
        MilestoneManager.OnMilestoneClaimed -= HandleMilestoneClaimed;
    }

    private void Start()
    {
        // Generate the full list of milestones when the UI is first shown
        PopulateInitialList();
    }

    private void PopulateInitialList()
    {
        List<MilestoneData> allMilestones = MilestoneManager.Instance.GetAllMilestones();

        foreach (MilestoneData milestone in allMilestones)
        {
            GameObject entry = Instantiate(milestoneEntryPrefab, listContainer);
            milestoneUIEntries[milestone.milestoneID] = entry;

            long currentProgress = MilestoneManager.Instance.GetProgress(milestone.milestoneID);
            bool isClaimed = MilestoneManager.Instance.IsClaimed(milestone.milestoneID);

            UpdateEntryUI(entry, milestone, currentProgress, isClaimed);
        }
    }

    private void UpdateMilestoneEntry(string milestoneID, long currentProgress, long goal)
    {
        if (milestoneUIEntries.ContainsKey(milestoneID))
        {
            GameObject entry = milestoneUIEntries[milestoneID];
            MilestoneData milestone = MilestoneManager.Instance.GetMilestoneByID(milestoneID);
            bool isClaimed = MilestoneManager.Instance.IsClaimed(milestoneID);
            
            UpdateEntryUI(entry, milestone, currentProgress, isClaimed);
        }
    }
    
    private void HandleMilestoneClaimed(MilestoneData claimedMilestone)
    {
        if (milestoneUIEntries.ContainsKey(claimedMilestone.milestoneID))
        {
            GameObject entry = milestoneUIEntries[claimedMilestone.milestoneID];
            long progress = MilestoneManager.Instance.GetProgress(claimedMilestone.milestoneID);

            // Update the UI to show it as claimed
            UpdateEntryUI(entry, claimedMilestone, progress, true);
        }
    }

    /// <summary>
    /// Central function to update a single UI entry's visuals based on its state.
    /// </summary>
    private void UpdateEntryUI(GameObject entry, MilestoneData milestone, long currentProgress, bool isClaimed)
    {
        // Find components on the prefab instance - cache these in a separate class for optimization
        TextMeshProUGUI nameText = entry.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI progressText = entry.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
        Slider progressBar = entry.transform.Find("ProgressBar").GetComponent<Slider>();
        Button claimButton = entry.transform.Find("ClaimButton").GetComponent<Button>();
        GameObject claimedOverlay = entry.transform.Find("ClaimedOverlay").gameObject;

        nameText.text = milestone.displayName;
        
        long goal = milestone.goal;
        float progressRatio = (float)currentProgress / goal;

        progressText.text = $"{currentProgress} / {goal}";
        progressBar.value = progressRatio;

        bool isComplete = currentProgress >= goal;

        if (isClaimed)
        {
            claimButton.gameObject.SetActive(false);
            claimedOverlay.SetActive(true);
        }
        else
        {
            claimedOverlay.SetActive(false);
            claimButton.gameObject.SetActive(true);
            claimButton.interactable = isComplete;

            // Wire up the button click event
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(() => {
                MilestoneManager.Instance.ClaimMilestone(milestone.milestoneID);
            });
        }
    }
}
