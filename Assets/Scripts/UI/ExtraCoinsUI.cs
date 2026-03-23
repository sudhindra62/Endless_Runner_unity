

using UnityEngine;
using UnityEngine.UI;

    public class ExtraCoinsUI : MonoBehaviour
    {
        public GameObject extraCoinsScreen;
        public Button watchAdButton;
        public Button noThanksButton;

        private void Start()
        {
            extraCoinsScreen.SetActive(false);
            watchAdButton.onClick.AddListener(WatchAdForExtraCoins);
            noThanksButton.onClick.AddListener(CloseScreen);
        }

        public void ShowExtraCoinsScreen()
        {
            extraCoinsScreen.SetActive(true);
        }

        private void WatchAdForExtraCoins()
        {
            AdManager.Instance.ShowRewardedAd(RewardType.ExtraCoins, (rewardType) =>
            {
                if (rewardType == RewardType.ExtraCoins)
                {
                    CloseScreen();
                }
            });
        }

        private void CloseScreen()
        {
            extraCoinsScreen.SetActive(false);
        }
    }

