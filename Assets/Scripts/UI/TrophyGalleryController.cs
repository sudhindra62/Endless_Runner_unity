using UnityEngine;

public class TrophyGalleryController : MonoBehaviour
{
    [SerializeField] private TrophyGallery trophyGallery;
    [SerializeField] private AchievementManager achievementManager;

    private void Start()
    {
        if (trophyGallery == null)
        {
            trophyGallery = FindFirstObjectByType<TrophyGallery>();
        }

        if (achievementManager == null)
        {
            achievementManager = FindFirstObjectByType<AchievementManager>();
        }
        
        // Initially hide the gallery
        HideTrophyGallery();
    }

    public void ShowTrophyGallery()
    {
        if (trophyGallery != null && achievementManager != null)
        {
            trophyGallery.Initialize(achievementManager.GetAchievementData());
            trophyGallery.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("TrophyGallery or AchievementManager not assigned in the TrophyGalleryController.");
        }
    }

    public void HideTrophyGallery()
    {
        if (trophyGallery != null)
        {
            trophyGallery.gameObject.SetActive(false);
        }
    }
}
