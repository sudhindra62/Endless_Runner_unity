
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Game";
    public GachaResultPanel gachaResultPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnGachaButtonPress()
    {
        LootLockerSDKManager.GetSingleAssetInstances(31, (response) =>
        {
            if (response.success)
            {
                // Assuming you have a way to display the result, e.g., a UI panel
                // For demonstration, let's log the details of the first asset
                if (response.instances.Length > 0)
                {
                    var asset = response.instances[0];
                    Debug.Log($"Gacha Drop: {asset.asset.name}");
                    gachaResultPanel.ShowGachaResultPanel(asset);

                }
            }
            else
            {
                Debug.LogError("Gacha failed: " + response.Error);
            }
        });
    }
}
