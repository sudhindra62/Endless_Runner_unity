
using UnityEngine;
using UnityEngine.UI;

public enum AchievementTier
{
    Bronze,
    Silver,
    Gold,
    Diamond
}

public class AchievementBadge : MonoBehaviour
{
    public Image badgeImage;

    public Color bronzeColor = new Color(0.8f, 0.5f, 0.2f);
    public Color silverColor = new Color(0.75f, 0.75f, 0.75f);
    public Color goldColor = new Color(1f, 0.84f, 0f);
    public Color diamondColor = new Color(0.7f, 0.95f, 1f);

    public void SetTier(AchievementTier tier)
    {
        switch (tier)
        {
            case AchievementTier.Bronze:
                badgeImage.color = bronzeColor;
                break;
            case AchievementTier.Silver:
                badgeImage.color = silverColor;
                break;
            case AchievementTier.Gold:
                badgeImage.color = goldColor;
                break;
            case AchievementTier.Diamond:
                badgeImage.color = diamondColor;
                break;
        }
    }
}
