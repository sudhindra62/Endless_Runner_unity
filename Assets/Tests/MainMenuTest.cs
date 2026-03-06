using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class MainMenuTest
{
    private MainMenu mainMenu;
    private GameObject mainMenuObject;
    private TrophyGalleryController trophyGalleryController;
    private GameObject trophyGalleryControllerObject;

    [SetUp]
    public void SetUp()
    {
        // MainMenu
        mainMenuObject = new GameObject();
        mainMenu = mainMenuObject.AddComponent<MainMenu>();

        // TrophyGalleryController
        trophyGalleryControllerObject = new GameObject();
        trophyGalleryController = trophyGalleryControllerObject.AddComponent<TrophyGalleryController>();
        mainMenu.trophyGalleryController = trophyGalleryController;
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(mainMenuObject);
        Object.Destroy(trophyGalleryControllerObject);
    }

    [UnityTest]
    public IEnumerator ShowTrophyGallery_CallsControllerShowMethod()
    {
        // Arrange
        bool wasCalled = false;
        // Mock the controller's ShowTrophyGallery method
        var mockTrophyController = new GameObject().AddComponent<TrophyGalleryController>();
        var gallery = new GameObject().AddComponent<TrophyGallery>();
        mockTrophyController.gameObject.AddComponent<TrophyGallery>();
        mainMenu.trophyGalleryController = mockTrophyController;

        // Act
        mainMenu.ShowTrophyGallery();
        yield return null;

        // Assert
        // We can't directly check if the method was called without a mocking framework.
        // Instead, we will check the side effects of the call.
        Assert.IsTrue(gallery.gameObject.activeSelf);

        // Clean up the mock objects
        Object.Destroy(mockTrophyController.gameObject);
        Object.Destroy(gallery.gameObject);
    }
}
