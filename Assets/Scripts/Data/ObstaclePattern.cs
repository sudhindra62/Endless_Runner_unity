using System;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public struct ObstaclePlacementData
    {
        public int laneIndex; // 0 for left, 1 for middle, 2 for right
        public GameObject obstaclePrefab;

        public ObstaclePlacementData(int lane, GameObject prefab)
        {
            laneIndex = lane;
            obstaclePrefab = prefab;
        }
    }

    [System.Serializable]
    public class ObstaclePattern
    {
        public List<ObstaclePlacementData> obstacles;
        public ObstaclePattern()
        {
            obstacles = new List<ObstaclePlacementData>();
        }
    }

