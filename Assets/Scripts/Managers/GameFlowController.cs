
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToMainMenu()
    {
        // Load Main Menu Scene
    }

    public void ToGame()
    {
        // Load Game Scene
    }

    public void ToShop()
    {
        // Load Shop Scene
    }
}
