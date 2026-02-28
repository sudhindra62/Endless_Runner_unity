using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        // Start a new game
        LevelManager.Instance.LoadLevel("SampleScene");
    }

    public void LoadGame()
    {
        // Load a saved game
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
