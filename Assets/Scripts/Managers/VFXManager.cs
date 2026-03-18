
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("Particle Effects")]
    [SerializeField] private GameObject runningTrails;
    [SerializeField] private GameObject coinCollectionSparkles;
    [SerializeField] private GameObject jumpEffect;
    [SerializeField] private GameObject enemyEffect;
    [SerializeField] private GameObject environmentEffect;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRunningTrails(Vector3 position)
    {
        if (runningTrails != null)
        {
            Instantiate(runningTrails, position, Quaternion.identity);
        }
    }

    public void PlayCoinCollectionSparkles(Vector3 position)
    {
        if (coinCollectionSparkles != null)
        {
            Instantiate(coinCollectionSparkles, position, Quaternion.identity);
        }
    }

    public void PlayJumpEffect(Vector3 position)
    {
        if (jumpEffect != null)
        {
            Instantiate(jumpEffect, position, Quaternion.identity);
        }
    }

    public void PlayEnemyEffect(Vector3 position)
    {
        if (enemyEffect != null)
        {
            Instantiate(enemyEffect, position, Quaternion.identity);
        }
    }

    public void PlayEnvironmentEffect(Vector3 position)
    {
        if (environmentEffect != null)
        {
            Instantiate(environmentEffect, position, Quaternion.identity);
        }
    }
}
