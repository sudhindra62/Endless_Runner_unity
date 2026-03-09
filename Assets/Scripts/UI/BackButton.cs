
using UnityEngine;

/// <summary>
/// A simple, reusable button that requests a return to the main menu.
/// This component is now fully compliant with the MODULARITY_MANDATE, 
/// delegating navigation logic to the centralized UIManager.
/// Refactored by Supreme Guardian Architect v12.
/// </summary>
[AddComponentMenu("UI/Buttons/Back To Main Menu Button")]
public class BackButton : MonoBehaviour
{
    /// <summary>
    /// Called by a UI Button'''s OnClick event.
    /// Finds the UIManager instance and calls its centralized navigation method.
    /// </summary>
    public void OnBackToMainMenuClicked()
    {
        // --- DELEGATION_MANDATE: Adherence to centralized control ---
        // The button is no longer aware of scenes; it only sends a command.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.GoToMainMenu();
        }
        else
        {   
            // --- ERROR_HANDLING_POLICY: Robust fallback for standalone testing ---
            Debug.LogError("Guardian Architect FATAL_ERROR: UIManager.Instance is not found. The BackButton cannot function.", this.gameObject);
        }
    }
}
