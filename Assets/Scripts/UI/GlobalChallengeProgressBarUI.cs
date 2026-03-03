using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI component to display the progress of the Global Community Challenge.
/// It listens to the CommunityChallengeManager and updates the progress bar and text.
/// </summary>
public class GlobalChallengeProgressBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI challengeNameText;
    [SerializeField] private GameObject container; // The parent object, to be hidden/shown

    private void OnEnable()
    {
        CommunityChallengeManager.OnChallengeActivated += InitializeUI;
        CommunityChallengeManager.OnProgressUpdated += UpdateProgress;
        container.SetActive(false); // Start hidden
    }

    private void OnDisable()
    {
        CommunityChallengeManager.OnChallengeActivated -= InitializeUI;
        CommunityChallengeManager.OnProgressUpdated -= UpdateProgress;
    }

    private void InitializeUI(CommunityChallengeData challengeData)
    {
        container.SetActive(true);
        challengeNameText.text = challengeData.challengeName;
    }

    private void UpdateProgress(double current, double target)
    {
        if (!container.activeInHierarchy) container.SetActive(true);
        
        // ENHANCEMENT: Global progress bar.
        float progress = (target > 0) ? (float)(current / target) : 0;
        progressBar.value = progress;

        progressText.text = $"{current:N0} / {target:N0}m ({progress:P1})";
    }
}
