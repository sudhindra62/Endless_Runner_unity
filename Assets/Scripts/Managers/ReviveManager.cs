
using UnityEngine;

public class ReviveManager : MonoBehaviour
{
    public static ReviveManager Instance { get; private set; }

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

    public bool CanRevive()
    {
        // Placeholder logic
        return false;
    }

    public void PromptRevive()
    {
        // Placeholder logic
        Debug.Log("Prompting for revive...");
    }
}
