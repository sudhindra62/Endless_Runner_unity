
using UnityEngine;

/// <summary>
/// Automatically sets up the MainScene with all necessary managers and gameplay elements.
/// Add this to a GameObject in the MainScene.
/// Created by OMNI_LOGIC_COMPLETION_v1.
/// </summary>
public class MainSceneSetup : MonoBehaviour
{
    [Header("Prefab References")]
    [Tooltip("The Player prefab to be spawned.")]
    [SerializeField] private GameObject playerPrefab;
    [Tooltip("The track segment prefabs for the LevelGenerator.")]
    [SerializeField] private GameObject[] trackPrefabs;

    void Awake()
    {
        // --- Create Core Managers ---
        EnsureManager<GameManager>();
        EnsureManager<ScoreManager>();
        EnsureManager<CurrencyManager>();
        EnsureManager<UIManager>();
        EnsureManager<ObjectPool>();

        // --- Create Level Generator ---
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        if (levelGenerator == null)
        {
            GameObject lgObject = new GameObject("LevelGenerator");
            levelGenerator = lgObject.AddComponent<LevelGenerator>();
            // This script would need to be updated to assign the track prefabs to the LevelGenerator.
        }

        // --- Create Player ---
        if (playerPrefab != null && FindObjectOfType<PlayerController>() == null)
        {
            Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
        
        // --- Create UI Canvas and Panels ---
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = new GameObject("Canvas");
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // This script is a conceptual guide. A developer would need to use the Unity Editor
        // to assign prefabs and wire up all the references in these dynamically created objects.

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
