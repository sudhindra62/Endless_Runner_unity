using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.Level
{
    public class TrackSegment : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        public Transform[] obstacleSlots;
        public Transform[] coinPaths;
        public GameObject prefab;

        private MasterObstacleSpawner _obstacleSpawner;
        private CoinSystem _coinSystem;

        private void Awake()
        {
            // ServiceLocator is used to retrieve core systems, ensuring loose coupling.
            _obstacleSpawner = ServiceLocator.Instance.Get<MasterObstacleSpawner>();
            _coinSystem = ServiceLocator.Instance.Get<CoinSystem>();
        }

        public Transform GetNextSpawnPoint()
        {
            return endPoint;
        }

        public void SpawnObstaclesAndCoins(ThemeConfig currentTheme)
        {
            if (_obstacleSpawner == null || _coinSystem == null)
            {
                Debug.LogError("Core systems not found. Ensure MasterObstacleSpawner and CoinSystem are registered with the ServiceLocator.");
                return;
            }

            var difficultyProfile = GameStateManager.Instance.GetCurrentDifficultyProfile();
            var patternData = ProceduralPatternEngine.Instance.GetPattern(difficultyProfile, currentTheme);

            _obstacleSpawner.SpawnObstaclesForPattern(patternData.ObstacleLayout, obstacleSlots, currentTheme.obstaclePrefabs);
            _coinSystem.SpawnCoinsForPattern(patternData.CoinLayout, coinPaths, currentTheme.coinPrefab);
        }
    }
}
