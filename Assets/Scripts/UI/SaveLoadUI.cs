using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    public SaveManager saveManager;

    public void SaveGameButton()
    {
        saveManager.SaveGame();
    }

    public void LoadGameButton()
    {
        saveManager.LoadGame();
    }
}
