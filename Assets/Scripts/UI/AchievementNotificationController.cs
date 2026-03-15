
using EndlessRunner.Achievements;
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.UI
{
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

        private void ShowNotification(Achievement achievement)
        {
            var notification = Instantiate(notificationPrefab, notificationContainer);
            notification.Setup(achievement);
            Destroy(notification.gameObject, notificationDisplayTime);
        }
    }
}
