

using UnityEngine;

    public class AchievementNotificationController : MonoBehaviour
    {
        [SerializeField] private AchievementItemUI notificationPrefab;
        [SerializeField] private Transform notificationContainer;
        [SerializeField] private float notificationDisplayTime = 3f;

        private void OnEnable()
        {
            GameEvents.OnAchievementUnlocked += ShowNotification;
        }

        private void OnDisable()
        {
            GameEvents.OnAchievementUnlocked -= ShowNotification;
        }

        private void ShowNotification(string achievementId)
        {
            Achievement achievement = AchievementManager.Instance != null
                ? AchievementManager.Instance.GetAchievementByID(achievementId)
                : null;
            if (achievement == null) return;

            var notification = Instantiate(notificationPrefab, notificationContainer);
            notification.Setup(achievement);
            Destroy(notification.gameObject, notificationDisplayTime);
        }
    }

