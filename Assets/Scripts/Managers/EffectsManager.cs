
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<EffectsManager>(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            ServiceLocator.Unregister<EffectsManager>();
            Instance = null;
        }
    }

    public void ConvertAllObstaclesToCoins()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        ObjectPooler objectPooler = ServiceLocator.Get<ObjectPooler>();

        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler not found. Cannot convert obstacles.");
            return;
        }

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null && obstacle.activeInHierarchy)
            {
                GameObject coin = objectPooler.GetFromPool("Coin", obstacle.transform.position, obstacle.transform.rotation);
                if (coin != null)
                {
                    coin.SetActive(true);
                }
                obstacle.SetActive(false);
            }
        }

        Debug.Log($"Converted {obstacles.Length} obstacles to coins.");
    }
}
