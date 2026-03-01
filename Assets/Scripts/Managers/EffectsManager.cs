
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    public void ConvertAllObstaclesToCoins()
    {
        Obstacle[] obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        ObjectPooler objectPooler = ServiceLocator.Get<ObjectPooler>();

        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler not found. Cannot convert obstacles.");
            return;
        }

        foreach (Obstacle obstacle in obstacles)
        {
            if (obstacle != null && obstacle.gameObject.activeInHierarchy)
            {
                GameObject coin = objectPooler.GetFromPool("Coin", obstacle.transform.position, obstacle.transform.rotation);
                if (coin != null)
                {
                    coin.SetActive(true);
                }
                obstacle.gameObject.SetActive(false);
            }
        }

        Debug.Log($"Converted {obstacles.Length} obstacles to coins.");
    }
}
