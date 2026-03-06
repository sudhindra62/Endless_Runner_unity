using UnityEngine;

public class PremiumVisualsManager : MonoBehaviour
{
    public static PremiumVisualsManager Instance { get; private set; }

    [Header("Impact Burst Settings")]
    public GameObject impactBurstPrefab;
    public float impactBurstLifetime = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayImpactBurst(Vector3 position)
    {
        if (impactBurstPrefab != null)
        {
            GameObject burst = Instantiate(impactBurstPrefab, position, Quaternion.identity);
            Destroy(burst, impactBurstLifetime);
        }
    }
}
