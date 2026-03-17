
using EndlessRunner.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class DailyRewardUI : MonoBehaviour
    {
        public GameObject rewardAvailableIcon;
        public Button claimRewardButton;

        private void Start()
        {
            claimRewardButton.onClick.AddListener(ClaimReward);
            UpdateUI();
        }

        private void UpdateUI()
        {
            bool isRewardAvailable = DailyRewardManager.Instance.IsRewardAvailable();
            rewardAvailableIcon.SetActive(isRewardAvailable);
            claimRewardButton.interactable = isRewardAvailable;
        }

        private void ClaimReward()
        {
            DailyRewardManager.Instance.ClaimReward();
            UpdateUI();
        }
    }
}
