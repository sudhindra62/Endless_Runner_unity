using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button newGameButton;
    public Button loadGameButton;
    public Button quitGameButton;

    void Start()
    {
        newGameButton.onClick.AddListener(MainMenu.Instance.NewGame);
        loadGameButton.onClick.AddListener(MainMenu.Instance.LoadGame);
        quitGameButton.onClick.AddListener(MainMenu.Instance.QuitGame);
    }
}
