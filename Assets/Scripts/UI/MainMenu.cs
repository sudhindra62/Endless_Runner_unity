
using UnityEngine;
using LootLocker.Requests;

/// <summary>
/// Handles UI interactions on the Main Menu screen.
/// Refactored by Supreme Guardian Architect v12 to delegate core logic to dedicated managers.
/// This script is now a pure View Controller.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The panel that displays the result of a gacha pull.")]
    public GachaResultPanel gachaResultPanel;

    private void OnEnable()
    {
        GachaManager.OnGachaResult += HandleGachaResult;
    }

    private void OnDisable()
    {
        GachaManager.OnGachaResult -= HandleGachaResult;
    }

    // --- PUBLIC UI-DRIVEN METHODS ---

    /// <summary>
    /// Called by a UI Button to initiate the game start sequence.
    /// This method now correctly calls the central GameManager.
    /// </summary>
    public void OnStartGamePressed()
    {
        GameManager.Instance.StartGame();
    }

    /// <summary>
    /// Called by a UI Button to quit the application.
    /// This method now correctly calls the central GameManager.
    /// </summary>
    public void OnQuitGamePressed()
    {
        GameManager.Instance.QuitGame();
    }

    /// <summary>
    /// Called by a UI Button to perform a gacha pull.
    /// This delegates the responsibility to the GachaManager.
    /// </summary>
    public void OnGachaButtonPressed()
    {
        if (GachaManager.Instance != null)
        {
            GachaManager.Instance.PerformGachaPull();
        }
        else
        { 
            Debug.LogError("GachaManager instance not found. Gacha pull cannot be performed.");
        }
    }

    // --- EVENT HANDLERS ---

    private void HandleGachaResult(LootLockerCommonAsset asset)
    {
        if (asset != null)
        {
            gachaResultPanel.ShowGachaResultPanel(asset);
        }
        else
        {
            // Optionally, show an error panel to the user
            Debug.LogWarning("Gacha result was null, not showing panel.");
        }
    }
}
