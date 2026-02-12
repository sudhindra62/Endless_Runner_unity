
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A centralized controller for managing the visibility of HUD elements.
/// This allows other systems to simply request the HUD to be shown or hidden
/// without needing direct references to each individual UI component.
/// 
/// --- Inspector Setup ---
/// 1. Attach this to a parent HUD Canvas or an empty GameObject.
/// 2. Drag all the UI elements that are part of the main game HUD (e.g., Score Text, Pause Button)
///    into the 'hudElements' list.
/// </summary>
public class HUDLayoutController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("A list of all GameObjects that should be shown/hidden as part of the HUD.")]
    [SerializeField] private List<GameObject> hudElements = new List<GameObject>();

    /// <summary>
    /// Shows all HUD elements assigned to the list.
    /// </summary>
    public void ShowHUD()
    {
        SetElementsActive(true);
    }

    /// <summary>
    /// Hides all HUD elements assigned to the list.
    /// </summary>
    public void HideHUD()
    {
        SetElementsActive(false);
    }

    private void SetElementsActive(bool isActive)
    {
        foreach (GameObject element in hudElements)
        {
            if (element != null)
            {
                element.SetActive(isActive);
            }
        }
    }
}
