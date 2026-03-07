
using UnityEngine;

/// <summary>
/// Automatically sets up the HomeScene with all necessary managers and UI elements.
/// Add this to a GameObject in the HomeScene.
/// Created by OMNI_LOGIC_COMPLETION_v1.
/// </summary>
public class HomeSceneSetup : MonoBehaviour
{
    void Awake()
    {
        // --- Create Core Managers ---
        EnsureManager<GameManager>();
        EnsureManager<ScoreManager>();
        EnsureManager<CurrencyManager>();
        EnsureManager<UIManager>();

        // --- Create UI Canvas and Panels ---
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) 
        {
            canvas = new GameObject("Canvas");
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // Ensure the UIManager is linked to the panels
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            // Create MainMenu Panel if it doesn't exist
            UIPanel_MainMenu mainMenuPanel = FindObjectOfType<UIPanel_MainMenu>();
            if (mainMenuPanel == null)
            {
                GameObject panelObj = new GameObject("UIPanel_MainMenu");
                panelObj.transform.SetParent(canvas.transform, false);
                mainMenuPanel = panelObj.AddComponent<UIPanel_MainMenu>();
            }
            // There would be more here to wire up the buttons and text fields.
            // This is a conceptual script as direct scene manipulation is not possible.
        }
        
        // Once setup is complete, this script can be disabled or destroyed
        Destroy(this);
    }

    private void EnsureManager<T>() where T : MonoBehaviour
    {
        if (FindObjectOfType<T>() == null)
        {
            GameObject managerObject = new GameObject(typeof(T).Name);
            managerObject.AddComponent<T>();
        }
    }
}
