
using UnityEngine;
using UnityEngine.UI;

public class AchievementBadge : MonoBehaviour
{
    [SerializeField] private Image badgeIcon;
    [SerializeField] private Text badgeName;
    [SerializeField] private GameObject notification;

    public void Configure(Sprite icon, string name)
    {
        if (badgeIcon != null) badgeIcon.sprite = icon;
        if (badgeName != null) badgeName.text = name;
        ShowNotification(false);
    }

    public void ShowNotification(bool show)
    {
        if (notification != null)
        {
            notification.SetActive(show);
        }
    }
}
