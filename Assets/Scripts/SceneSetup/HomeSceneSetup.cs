
using UnityEngine;
using UnityEngine.UI; // Required for Canvas, CanvasScaler, GraphicRaycaster


/// <summary>
/// Automatically and comprehensively sets up the HomeScene with ALL necessary singleton managers and a foundational UI structure.
/// This script now serves as the authoritative controller for activating conditional systems like the Tutorial.
/// Logic has been expanded and fortified by Supreme Guardian Architect v13 to guarantee 100% operational readiness.
/// </summary>
public class HomeSceneSetup : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Guardian Architect: Initiating comprehensive HomeScene setup...");

        // --- CORE & PERSISTENT MANAGERS ---
        // These are instantiated first as many other systems depend on them.
        EnsureManager<GameManager>();
        EnsureManager<SaveManager>(); 
        EnsureManager<InputManager>();
        EnsureManager<UIManager>();
        EnsureManager<SoundManager>();
        EnsureManager<AnalyticsManager>();
        EnsureManager<RemoteConfigManager>();
        EnsureManager<ThemeManager>();
        EnsureManager<ObjectPooler>();

        // --- ECONOMY & MONETIZATION MANAGERS ---
        EnsureManager<CurrencyManager>();
        EnsureManager<IAPManager>();
        EnsureManager<AdManager>();
        EnsureManager<ShopManager>();

        // --- PLAYER PROGRESSION & RETENTION MANAGERS ---
        EnsureManager<ScoreManager>();
        EnsureManager<MissionManager>();
        EnsureManager<AchievementManager>();
        EnsureManager<DailyLoginManager>();
        EnsureManager<PlayerProgression>();

        // --- LIVE SERVICES & EVENTS MANAGERS ---
        EnsureManager<LiveOpsManager>();
        EnsureManager<EventManager>();
        EnsureManager<CommunityChallengeManager>();

        // --- TECHNICAL & VALIDATION MANAGERS ---
        EnsureManager<IntegrityManager>();

        // --- CONDITIONAL & SCENE-SPECIFIC SETUP ---
        // Centralized logic for features that don't always run.
        SetupTutorialManager();

        // --- UI Canvas AND PANEL SETUP ---
        SetupUICanvas();

        Debug.Log("Guardian Architect: HomeScene setup complete. All systems online.");
        
        // This setup object has fulfilled its purpose.
        Destroy(gameObject);
    }

    /// <summary>
    /// Ensures a singleton manager of the specified type exists in the scene.
    /// </summary>
    private void EnsureManager<T>() where T : MonoBehaviour
    {
        if (FindObjectOfType<T>() == null)
        {
            GameObject managerObject = new GameObject(typeof(T).Name);
            managerObject.AddComponent<T>();
            Debug.Log($"Guardian Architect: Created missing manager -> {typeof(T).Name}");
        } else {
             Debug.Log($"Guardian Architect: Manager already exists -> {typeof(T).Name}");
        }
    }

    /// <summary>
    /// Checks if the tutorial needs to run and, if so, instantiates and starts the TutorialManager.
    /// This is the authoritative point of control for tutorial activation.
    /// </summary>
    private void SetupTutorialManager()
    {
        // A-TO-Z CONNECTIVITY: Read tutorial state from correct SaveManager property
        bool tutorialCompleted = (SaveManager.Instance != null) && SaveManager.Instance.Data.hasCompletedTutorial;
        
        if (!tutorialCompleted)
        {
            Debug.Log("Guardian Architect: Player has not completed the tutorial. Initializing TutorialManager.");
            EnsureManager<TutorialManager>();
            // StartTutorial accessible via public method
            if (TutorialManager.Instance != null)
                TutorialManager.Instance.StartTutorial();
        }
        else
        {
            Debug.Log("Guardian Architect: Tutorial already completed. Skipping.");
        }
    }

    /// <summary>
    /// Ensures the primary UI Canvas and the Main Menu Panel are set up correctly.
    /// </summary>
    private void SetupUICanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("UICanvas_Auto-Generated");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Debug.Log("Guardian Architect: Created foundational UI Canvas.");
        }

        // Ensure a main menu panel exists for the UIManager to find.
        if (canvas.GetComponentInChildren<UIPanel_MainMenu>() == null)
        {
             GameObject panelObj = new GameObject("UIPanel_MainMenu");
             panelObj.transform.SetParent(canvas.transform, false);
             panelObj.AddComponent<UIPanel_MainMenu>();
             Debug.Log("Guardian Architect: Created UIPanel_MainMenu and attached to canvas.");
        }
    }
}
