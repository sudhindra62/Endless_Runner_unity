using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager Prefabs")]
    [SerializeField] private GameObject buildSettingsPrefab;
    [SerializeField] private GameObject playerDataManagerPrefab;
    [SerializeField] private GameObject scoreManagerPrefab;
    [SerializeField] private GameObject sceneControllerPrefab;
    [SerializeField] private GameObject firebaseManagerPrefab;
    [SerializeField] private GameObject adMobManagerPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeManagers();
    }

    private void InitializeManagers()
    {
        InitializeManager<BuildSettings>(buildSettingsPrefab);
        InitializeManager<PlayerDataManager>(playerDataManagerPrefab);
        InitializeManager<ScoreManager>(scoreManagerPrefab);
        InitializeManager<SceneController>(sceneControllerPrefab);
        InitializeManager<FirebaseManager>(firebaseManagerPrefab);
        InitializeManager<AdMobManager>(adMobManagerPrefab);

        // Initialize managers that depend on BuildSettings after it's been initialized
        if (BuildSettings.Instance != null)
        {
            FirebaseManager.Instance?.Initialize();
            AdMobManager.Instance?.Initialize();
        }

        Debug.Log("[GameManager] All core managers initialized.");
    }

    private void InitializeManager<T>(GameObject prefab) where T : MonoBehaviour
    {
        if (FindFirstObjectByType<T>() == null)
        {
            if (prefab != null)
            {
                Instantiate(prefab, transform);
                Debug.Log($"[GameManager] Instantiated {typeof(T).Name} from prefab.");
            }
            else
            {
                Debug.LogWarning($"[GameManager] No instance of {typeof(T).Name} found and no prefab was provided.");
            }
        }
    }

    public void CompleteChallenge()
    {
        PlayerDataManager.Instance.AddGems(5);
    }
}
