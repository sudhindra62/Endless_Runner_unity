using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the Daily Login Reward UI panel.
/// </summary>
public class DailyLoginUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardAmountText;
    [SerializeField] private Button claimButton;

    void Start()
    {
        claimButton.onClick.AddListener(OnClaimButtonPressed);
        DailyLoginManager.OnRewardStateChanged += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        DailyLoginManager.OnRewardStateChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (DailyLoginManager.Instance.IsRewardAvailable)
        {
            LoginRewardData rewardData = DailyLoginManager.Instance.GetTodayRewardData();
            if (rewardData != null)
            {
                dayText.text = $"Day {DailyLoginManager.Instance.CurrentStreak}";
                rewardIcon.sprite = rewardData.rewardIcon;
                rewardAmountText.text = rewardData.amount.ToString();
                rewardPanel.SetActive(true);
            }
            else
            {
                rewardPanel.SetActive(false);
            }
        }
        else
        {
            rewardPanel.SetActive(false);
        }
    }

    private void OnClaimButtonPressed()
    {
        DailyLoginManager.Instance.ClaimReward();
    }
}
