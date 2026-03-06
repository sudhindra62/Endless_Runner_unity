using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    public TrophyGallery trophyGallery;

    void Start()
    {
        // Ensure the gallery is hidden on start
        trophyGallery.gameObject.SetActive(false);
    }

    public void ShowTrophyGallery()
    {
        // Populate the gallery with the latest achievement data
        trophyGallery.Initialize(AchievementManager.Instance.achievements);
        trophyGallery.gameObject.SetActive(true);
    }

    public void HideTrophyGallery()
    {
        trophyGallery.gameObject.SetActive(false);
    }
}
