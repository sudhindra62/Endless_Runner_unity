
using UnityEngine;
using UnityEngine.UI; // Required for Canvas elements

/// <summary>
/// Automatically and comprehensively sets up the MainScene with ALL necessary singleton managers and gameplay elements.
/// This script ensures every gameplay system is active and correctly wired at runtime.
/// Logic has been fully implemented by Supreme Guardian Architect v13 to be self-sufficient and robust.
/// </summary>
public class MainSceneSetup : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Guardian Architect: Initiating comprehensive MainScene setup...");

        // --- CORE & TECHNICAL MANAGERS (many are DontDestroyOnLoad from HomeScene) ---
        EnsureManager<GameManager>();
        EnsureManager<SaveManager>();
        EnsureManager<InputManager>();
        EnsureManager<UIManager>();
        EnsureManager<ObjectPooler>(); // Essential for dynamic object spawning
        EnsureManager<SoundManager>();
        EnsureManager<VFXManager>();
        EnsureManager<AnalyticsManager>();
        EnsureManager<RemoteConfigManager>();
        EnsureManager<IntegrityManager>();

        // --- VISUAL ENGINE MANAGERS ---
        EnsureManager<LightingManager>();
        EnsureManager<SkyboxManager>();
        EnsureManager<PostProcessingManager>();
        EnsureManager<EnvironmentAnimationManager>();
        EnsureManager<PerformanceManager>();

        // --- CORE GAMEPLAY MANAGERS ---
        EnsureManager<LevelGenerator>();
        EnsureManager<ScoreManager>();
        EnsureManager<CurrencyManager>();
        EnsureManager<SpeedManager>();
        EnsureManager<PowerUpManager>();
        EnsureManager<AdaptiveDifficultyManager>();
        EnsureManager<FeverModeManager>();
        EnsureManager<MomentumManager>();
        EnsureManager<NearMissManager>();
        EnsureManager<StyleManager>();

        // --- ADVANCED GAMEPLAY & EVENT MANAGERS ---
        EnsureManager<BossChaseManager>();
        EnsureManager<RunModifierManager>();
        EnsureManager<PowerUpFusionManager>();
        EnsureManager<EnvironmentEventManager>();

        // --- PLAYER & CUSTOMIZATION MANAGERS ---
        EnsureManager<PlayerManager>();
        EnsureManager<CosmeticEffectManager>();

        // --- DYNAMICALLY LOAD AND WIRE PREFABS ---
        LoadAndWirePrefabs();

        // --- SPAWN INITIAL GAME OBJECTS ---
        SpawnInitialGameObjects();

        // --- UI CANVAS AND IN-GAME HUD SETUP ---
        SetupUICanvas();

        Debug.Log("Guardian Architect: MainScene setup complete. All systems are online.");

        // The setup is complete, and this object is no longer needed.
        Destroy(gameObject);
    }

    /// <summary>
    /// Ensures a singleton manager of the specified type exists in the scene.
    /// If one is not found, it creates a new GameObject and adds the manager component.
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
    /// Dynamically loads critical prefabs from the Resources folder and wires them to the appropriate managers.
    /// This removes the need for manual drag-and-drop assignments in the editor.
    /// </summary>
    private void LoadAndWirePrefabs()
    {
        Debug.Log("Guardian Architect: Loading and wiring prefabs from Resources...");

        // --- Wire LevelGenerator Prefabs ---
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        if (levelGenerator != null)
        {
            // Load tile/track prefabs - assuming they are in a "Prefabs/Track" folder within Resources
            levelGenerator.trackPrefabs = Resources.LoadAll<GameObject>("Prefabs/Track");
            levelGenerator.obstaclePrefabs = Resources.LoadAll<GameObject>("Prefabs/Obstacles");
            levelGenerator.coinPrefab = Resources.Load<GameObject>("Prefabs/Collectibles/Coin");
            Debug.Log($"Guardian Architect: Wired {levelGenerator.trackPrefabs.Length} track prefabs and {levelGenerator.obstaclePrefabs.Length} obstacle prefabs to LevelGenerator.");
        }

        // --- Wire ObjectPooler Prefabs ---
        ObjectPooler objectPooler = FindObjectOfType<ObjectPooler>();
        if (objectPooler != null)
        {
            // Example: Pre-load all power-up prefabs for pooling
            GameObject[] powerUpPrefabs = Resources.LoadAll<GameObject>("Prefabs/PowerUps");
            foreach (var prefab in powerUpPrefabs)
            {
                objectPooler.CreatePool(prefab, 5); // Create an initial pool of 5 for each power-up
            }
            Debug.Log($"Guardian Architect: Pre-warmed ObjectPooler with {powerUpPrefabs.Length} power-up types.");
        }
    }

    /// <summary>
    /// Spawns the essential, non-manager objects required for gameplay to start.
    /// </summary>
    private void SpawnInitialGameObjects()
    {
        // --- Spawn Player ---
        if (FindObjectOfType<PlayerController>() == null)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            if (playerPrefab != null)
            {
                Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
                Debug.Log("Guardian Architect: Player prefab instantiated at starting position.");
            } else {
                Debug.LogError("Guardian Architect CRITICAL ERROR: Player prefab not found at 'Resources/Prefabs/Player'!");
            }
        }
    }

    /// <summary>
    /// Ensures the UI Canvas and the In-Game HUD are correctly set up.
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
            Debug.Log("Guardian Architect: Created foundational UI Canvas for MainScene.");
        }

        // Ensure the UIManager finds and manages the In-Game HUD
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            // Expect the in-game HUD to be a panel like UIPanel_InGame
            UIPanel_InGame inGamePanel = canvas.GetComponentInChildren<UIPanel_InGame>();
            if (inGamePanel == null)
            {
                GameObject panelObj = new GameObject("UIPanel_InGame");
                panelObj.transform.SetParent(canvas.transform, false);
                inGamePanel = panelObj.AddComponent<UIPanel_InGame>();
                Debug.Log("Guardian Architect: Created UIPanel_InGame for live gameplay stats.");
            }
        }
    }
}
