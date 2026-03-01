using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RunModifierUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject modifierPanel; // The container for the icons
    [SerializeField] private GameObject modifierIconPrefab; // Prefab with an Image and a Tooltip component

    private List<GameObject> activeIcons = new List<GameObject>();

    private void Start()
    {
        RunModifierManager.OnModifiersApplied += UpdateModifierDisplay;
        GameManager.OnGameStateChanged += OnGameStateChanged;

        modifierPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        RunModifierManager.OnModifiersApplied -= UpdateModifierDisplay;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        // Hide the panel when the run ends or we go back to the menu
        if (newState == GameState.EndOfRun || newState == GameState.Menu)
        {
            ClearModifierDisplay();
            modifierPanel.SetActive(false);
        }
    }

    private void UpdateModifierDisplay(List<RunModifierData> modifiers)
    {
        ClearModifierDisplay();

        if (modifiers.Count == 0)
        {
            modifierPanel.SetActive(false);
            return;
        }

        modifierPanel.SetActive(true);
        foreach (var modifierData in modifiers)
        {
            GameObject iconInstance = Instantiate(modifierIconPrefab, modifierPanel.transform);
            // Assuming the prefab has a script to set icon and tooltip, e.g., ModifierIconUI
            // var iconUI = iconInstance.GetComponent<ModifierIconUI>();
            // if(iconUI != null) 
            // {
            //    iconUI.Setup(modifierData);
            // }
            
            // Simple version for demonstration:
            var text = iconInstance.GetComponentInChildren<TextMeshProUGUI>();
            if(text != null) text.text = modifierData.displayName;

            activeIcons.Add(iconInstance);
        }
    }

    private void ClearModifierDisplay()
    {
        foreach (var icon in activeIcons)
        {
            Destroy(icon);
        }
        activeIcons.Clear();
    }
}
