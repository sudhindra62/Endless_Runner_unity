using UnityEngine;

public class BuildSettings : MonoBehaviour
{
    public static BuildSettings Instance { get; private set; }

    [Header("Build Settings")]
    [SerializeField] private bool isProductionBuild = false;
    [SerializeField] private bool adsEnabled = true;

    public bool IsProductionBuild => isProductionBuild;
    public bool AdsEnabled => adsEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
