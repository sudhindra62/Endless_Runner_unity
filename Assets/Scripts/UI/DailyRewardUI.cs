
using EndlessRunner.Managers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EndlessRunner.UI
{
    public class DailyRewardUI : MonoBehaviour
    {
        public GameObject rewardScreen;
        public Button openRewardButton;
        public Button claimRewardButton;
        public Button closeButton;
        public TextMeshProUGUI rewardText;

        private Reward dailyReward;

        private void Start()
        {
            openRewardButton.onClick.AddListener(OpenRewardScreen);
            claimRewardButton.onClick.AddListener(ClaimReward);
            closeButton.onClick.AddListener(CloseRewardScreen);
            
            rewardScreen.SetActive(false);
            UpdateUI();
        }

        private void UpdateUI()
        {
            bool isRewardAvailable = DailyRewardManager.Instance.IsRewardAvailable();
            openRewardButton.gameObject.SetActive(isRewardAvailable);
        }

        private void OpenRewardScreen()
        {
            dailyReward = DailyRewardManager.Instance.GetRandomReward();
            UpdateRewardDescription();
            rewardScreen.SetActive(true);
            claimRewardButton.interactable = true;
        }

        private void UpdateRewardDescription()
        {
            switch (dailyReward.rewardType)
            {
                case DailyRewardType.Gems:
                    rewardText.text = $"Claim your daily reward: {dailyReward.amount} Gems!";
                    break;
                case DailyRewardType.Coins:
                    rewardText.text = $"Claim your daily reward: {dailyReward.amount} Coins!";
                    break;
                case DailyRewardType.TemporaryTheme:
                    rewardText.text = $"Claim your daily reward: Temporary unlock of '{dailyReward.temporaryTheme.themeName}' for {dailyReward.temporaryThemeDurationHours} hours!";
                    break;
            }
        }

        private void ClaimReward()
        {
            DailyRewardManager.Instance.ClaimReward(dailyReward);
            claimRewardButton.interactable = false;
            rewardText.text = "Reward Claimed!";
            UpdateUI();
        }

        private void CloseRewardScreen()
        {
            rewardScreen.SetActive(false);
        }
    }
}
