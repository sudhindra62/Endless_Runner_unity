using UnityEngine;
using System.Collections.Generic;

public class ObstaclePool : MonoBehaviour
{
    public static ObstaclePool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetObstacle(GameObject obstaclePrefab)
    {
        if (obstaclePools.ContainsKey(obstaclePrefab) && obstaclePools[obstaclePrefab].Count > 0)
        {
            GameObject obstacle = obstaclePools[obstaclePrefab].Dequeue();
            obstacle.SetActive(true);
            return obstacle;
        }
        else
        {
            GameObject newObstacle = Instantiate(obstaclePrefab);
            return newObstacle;
        }
    }

    public void ReturnObstacle(GameObject obstacle, GameObject obstaclePrefab)
    {
        obstacle.SetActive(false);

        if (!obstaclePools.ContainsKey(obstaclePrefab))
        {
            obstaclePools[obstaclePrefab] = new Queue<GameObject>();
        }

        obstaclePools[obstaclePrefab].Enqueue(obstacle);
    }
}
