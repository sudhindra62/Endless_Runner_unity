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

    private void InitializeUI()
    {
        container.SetActive(true);
        if (challengeNameText != null)
        {
            challengeNameText.text = "Global Challenge";
        }
    }

    private void UpdateProgress(float progress)
    {
        if (!container.activeInHierarchy) container.SetActive(true);
        progressBar.value = progress;
        progressText.text = $"{progress:P1}";
    }
}
