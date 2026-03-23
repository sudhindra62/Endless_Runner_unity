
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BonusRunUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonusRunsText;
    [SerializeField] private Button purchaseBonusRunsButton; // Monetization option
    [SerializeField] private Button watchAdForBonusRunButton; // Ad monetization option

    private void OnEnable()
    {
        BonusRunManager.OnBonusRunsChanged += UpdateBonusRunsText;
    }

    private void OnDisable()
    {
        BonusRunManager.OnBonusRunsChanged -= UpdateBonusRunsText;
    }

    private void Start()
    {
        // Initialize with current bonus runs
        if (BonusRunManager.Instance != null)
        {
            UpdateBonusRunsText(BonusRunManager.Instance.GetBonusRunsRemaining());
        }

        if (purchaseBonusRunsButton != null)
        {
            purchaseBonusRunsButton.onClick.AddListener(OnPurchaseBonusRuns);
        }

        if (watchAdForBonusRunButton != null)
        {
            watchAdForBonusRunButton.onClick.AddListener(OnWatchAdForBonusRun);
        }
    }

    private void UpdateBonusRunsText(int runsRemaining)
    {
        if (bonusRunsText != null)
        {
            bonusRunsText.text = "Bonus Runs: " + runsRemaining;
        }
    }

    private void OnPurchaseBonusRuns()
    {
        // In a real game, this would trigger an in-app purchase flow
        Debug.Log("Purchase bonus runs clicked");
        BonusRunManager.Instance.AddBonusRuns(5);
    }

    private void OnWatchAdForBonusRun()
    {
        // In a real game, this would trigger a rewarded ad flow
        Debug.Log("Watch ad for bonus run clicked");
        BonusRunManager.Instance.AddBonusRuns(1);
    }
}
