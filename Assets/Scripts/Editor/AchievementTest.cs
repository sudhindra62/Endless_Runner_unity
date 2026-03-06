
using UnityEditor;
using UnityEngine;

public class AchievementTest : EditorWindow
{
    [MenuItem("Window/Achievement Test")]
    public static void ShowWindow()
    {
        GetWindow<AchievementTest>("Achievement Test");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Test Unlock Achievement"))
        {
            TestUnlockAchievement();
        }
    }

    private void TestUnlockAchievement()
    {
        // Make sure the game is in a state where the AchievementManager can be accessed
        if (Application.isPlaying && AchievementManager.Instance != null)
        {
            // Replace with a valid achievement ID from your configuration
            string testAchievementId = "run_1000m"; 
            AchievementManager.Instance.UnlockAchievement(testAchievementId);
        }
        else
        {
            Debug.LogError("Achievement test requires the game to be running with an active AchievementManager.");
        }
    }
}
