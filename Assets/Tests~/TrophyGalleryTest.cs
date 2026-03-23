using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Collections;

public class TrophyGalleryTest
{
    private TrophyGalleryController controller;
    private TrophyGallery gallery;
    private AchievementManager achievementManager;
    private GameObject controllerObject;
    private GameObject galleryObject;
    private GameObject achievementManagerObject;
    private GameObject trophyPrefab;

    [SetUp]
    public void SetUp()
    {
        // Controller
        controllerObject = new GameObject();
        controller = controllerObject.AddComponent<TrophyGalleryController>();

        // Gallery
        galleryObject = new GameObject();
        gallery = galleryObject.AddComponent<TrophyGallery>();
        controller.gameObject.AddComponent<TrophyGallery>(); // Add to controller to be found by FindObjectOfType
        
        // AchievementManager
        achievementManagerObject = new GameObject();
        achievementManager = achievementManagerObject.AddComponent<AchievementManager>();
        controller.gameObject.AddComponent<AchievementManager>(); // Add to controller to be found by FindObjectOfType

        // Trophy Prefab with required components
        trophyPrefab = new GameObject();
        trophyPrefab.AddComponent<Image>();
        GameObject icon = new GameObject("Icon");
        icon.transform.SetParent(trophyPrefab.transform);
        icon.AddComponent<Image>();
        GameObject name = new GameObject("Name");
        name.transform.SetParent(trophyPrefab.transform);
        name.AddComponent<Text>();
        GameObject description = new GameObject("Description");
        description.transform.SetParent(trophyPrefab.transform);
        description.AddComponent<Text>();
        gallery.trophyPrefab = trophyPrefab;

        // Container for trophies
        gallery.trophyContainer = new GameObject().transform;

        // Set achievement data
        AchievementData data = ScriptableObject.CreateInstance<AchievementData>();
        data.achievements = new System.Collections.Generic.List<Achievement>
        {
            new Achievement { name = "Test1", description = "Desc1", goal = 1, isUnlocked = true },
            new Achievement { name = "Test2", description = "Desc2", goal = 2, isUnlocked = false }
        };
        achievementManager.SetAchievementData(data);

        // Public field assignments for the test
        galleryObject.SetActive(false); // Start with gallery hidden
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(controllerObject);
        Object.Destroy(galleryObject);
        Object.Destroy(achievementManagerObject);
        Object.Destroy(trophyPrefab);
        Object.Destroy(gallery.trophyContainer.gameObject);
    }

    [UnityTest]
    public IEnumerator ShowTrophyGallery_ActivatesGalleryAndPopulatesTrophies()
    {
        // Arrange
        gallery.gameObject.SetActive(false);

        // Act
        controller.ShowTrophyGallery();
        yield return null;

        // Assert
        Assert.IsTrue(gallery.gameObject.activeSelf);
        Assert.AreEqual(2, gallery.trophyContainer.childCount);

        // Check first trophy (unlocked)
        Transform firstTrophy = gallery.trophyContainer.GetChild(0);
        Assert.AreEqual("Trophy_Test1", firstTrophy.name);
        Assert.AreEqual(gallery.unlockedColor, firstTrophy.Find("Icon").GetComponent<Image>().color);

        // Check second trophy (locked)
        Transform secondTrophy = gallery.trophyContainer.GetChild(1);
        Assert.AreEqual("Trophy_Test2", secondTrophy.name);
        Assert.AreEqual(gallery.lockedColor, secondTrophy.Find("Icon").GetComponent<Image>().color);
    }

    [UnityTest]
    public IEnumerator HideTrophyGallery_DeactivatesGallery()
    {
        // Arrange
        gallery.gameObject.SetActive(true);

        // Act
        controller.HideTrophyGallery();
        yield return null;

        // Assert
        Assert.IsFalse(gallery.gameObject.activeSelf);
    }
}
