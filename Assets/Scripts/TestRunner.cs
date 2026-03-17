
using UnityEngine;
using EndlessRunner.Character;
using EndlessRunner.Cinematics;
using EndlessRunner.Managers;

public class TestRunner : MonoBehaviour
{
    public void RunTests()
    {
        Debug.Log("Starting tests...");

        TestCharacterSkins();
        TestCinematics();

        Debug.Log("Tests finished.");
    }

    private void TestCharacterSkins()
    {
        Debug.Log("--- Testing Character Skins ---");

        // Add a test skin to the manager for testing purposes
        CharacterSkin testSkin = ScriptableObject.CreateInstance<CharacterSkin>();
        testSkin.characterName = "Test Character";
        testSkin.unlockType = SkinUnlockType.Coins;
        testSkin.price = 100;
        CharacterSkinManager.Instance.allSkins.Add(testSkin);

        // Test initial state
        Debug.Log("Initial selected skin: " + CharacterSkinManager.Instance.GetSelectedSkin().characterName);
        Debug.Log("Is 'Test Character' unlocked? " + CharacterSkinManager.Instance.IsSkinUnlocked(testSkin));

        // Test unlocking
        CurrencyManager.Instance.AddCoins(150);
        Debug.Log("Unlocking 'Test Character'...");
        bool unlocked = CharacterSkinManager.Instance.UnlockSkin(testSkin);
        Debug.Log("'Test Character' unlocked: " + unlocked);
        Debug.Log("Is 'Test Character' unlocked now? " + CharacterSkinManager.Instance.IsSkinUnlocked(testSkin));

        // Test selecting
        Debug.Log("Selecting 'Test Character'...");
        CharacterSkinManager.Instance.SetSelectedSkin(testSkin);
        Debug.Log("Selected skin: " + CharacterSkinManager.Instance.GetSelectedSkin().characterName);
    }

    private void TestCinematics()
    {
        Debug.Log("--- Testing Cinematics ---");

        // The cinematic manager is mostly visual, but we can test if it plays
        Debug.Log("Playing pre-run cinematic...");
        if (PreRunCinematicManager.Instance != null)
        {
            PreRunCinematicManager.Instance.PlayCinematic();
        }
        else
        {
            Debug.LogWarning("PreRunCinematicManager not found in scene.");
        }
    }
}
