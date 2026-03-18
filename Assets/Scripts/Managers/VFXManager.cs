
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("Particle Effect Prefabs")]
    [SerializeField] private GameObject runningTrails;
    [SerializeField] private GameObject coinCollectionSparkles;
    [SerializeField] private GameObject jumpEffect;
    [SerializeField] private GameObject enemyEffect;
    [SerializeField] private GameObject environmentEffect;

    private ObjectPooler objectPooler;

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

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        // Pre-warm the pools for effects that are used frequently
        if (objectPooler != null)
        {
            if(runningTrails != null) objectPooler.CreatePool(runningTrails, 10);
            if(coinCollectionSparkles != null) objectPooler.CreatePool(coinCollectionSparkles, 20);
            if(jumpEffect != null) objectPooler.CreatePool(jumpEffect, 5);
        }
    }

    private void PlayEffect(GameObject effectPrefab, Vector3 position)
    {
        if (effectPrefab == null || objectPooler == null) return;

        GameObject effect = objectPooler.GetPooledObject(effectPrefab);
        if (effect != null)
        {
            effect.transform.position = position;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
            // A component on the particle system prefab should handle returning it to the pool
        }
    }

    public void PlayRunningTrails(Vector3 position)
    {
        PlayEffect(runningTrails, position);
    }

    public void PlayCoinCollectionSparkles(Vector3 position)
    {
        PlayEffect(coinCollectionSparkles, position);
    }

    public void PlayJumpEffect(Vector3 position)
    {
        PlayEffect(jumpEffect, position);
    }

    public void PlayEnemyEffect(Vector3 position)
    {
        PlayEffect(enemyEffect, position);
    }

    public void PlayEnvironmentEffect(Vector3 position)
    {
        PlayEffect(environmentEffect, position);
    }
}
