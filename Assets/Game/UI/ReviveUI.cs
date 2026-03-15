
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Managers;
using EndlessRunner.Player;

namespace EndlessRunner.UI
{
    public class ReviveUI : MonoBehaviour
    {
        public Button watchAdButton;
        public Button useGemsButton;
        public Button noThanksButton;
        public int gemsForRevive = 50;

        void Start()
        {
            watchAdButton.onClick.AddListener(OnWatchAdClicked);
            useGemsButton.onClick.AddListener(OnUseGemsClicked);
            noThanksButton.onClick.AddListener(OnNoThanksClicked);
        }

        void OnWatchAdClicked()
        {
            AdManager.Instance.ShowRewardedVideo(() =>
            {
                // Player watched the ad, revive the player
                // Add your revive logic here
                gameObject.SetActive(false);
            });
        }

        void OnUseGemsClicked()
        {
            if (PlayerCurrency.Instance.SpendGems(gemsForRevive))
            {
                // Player has enough gems, revive the player
                // Add your revive logic here
                gameObject.SetActive(false);
            }
            else
            {
                // Not enough gems
                Debug.Log("Not enough gems to revive!");
            }
        }

        void OnNoThanksClicked()
        {
            // Player chose not to revive, end the run
            // Add your end run logic here
            gameObject.SetActive(false);
        }
    }
}
