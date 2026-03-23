
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public class AchievementItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressBar;
        [SerializeField] private Button claimButton;
        [SerializeField] private GameObject completedOverlay;

        private Achievement achievement;

        public void Setup(Achievement ach)
        {
            achievement = ach;
            claimButton.onClick.AddListener(OnClaimButtonPressed);
            Refresh();
        }

        public void Refresh()
        {
            titleText.text = achievement.title;
            descriptionText.text = achievement.description;

            if (achievement.isUnlocked)
            {
                progressBar.fillAmount = 1f;
                progressText.text = "Completed";
                claimButton.gameObject.SetActive(!achievement.isRewardClaimed);
                completedOverlay.SetActive(achievement.isRewardClaimed);
            }
            else
            {
                // In a more advanced implementation, you might get the current progress
                // from the AchievementManager to show a partial progress bar.
                progressBar.fillAmount = 0f;
                progressText.text = $"0 / {achievement.requiredValue}";
                claimButton.gameObject.SetActive(false);
                completedOverlay.SetActive(false);
            }
        }

        private void OnClaimButtonPressed()
        {
            AchievementManager.Instance.ClaimReward(achievement.id);
            Refresh();
        }
    }
