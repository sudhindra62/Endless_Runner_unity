
using System.Collections.Generic;
using UnityEngine;
using Game.Themes;

namespace Game.Environment
{
    public class EnvironmentVariationManager : MonoBehaviour
    {
        public static EnvironmentVariationManager Instance;

        [SerializeField]
        private Transform playerTransform;
        [SerializeField]
        private float spawnZ = 0f;
        [SerializeField]
        private float tileLength = 100f;
        [SerializeField]
        private int amnTilesOnScreen = 3;
        [SerializeField]
        private float despawnZ = -10f;

        private List<GameObject> activeTiles;
        private ThemeManager themeManager;

        private void Awake()
        {
            Instance = this;
            activeTiles = new List<GameObject>();
        }

        private void Start()
        {
            themeManager = ThemeManager.Instance;
            for (int i = 0; i < amnTilesOnScreen; i++)
            {
                SpawnTile();
            }
        }

        private void Update()
        {
            if (playerTransform.position.z - despawnZ > spawnZ - (amnTilesOnScreen * tileLength))
            {
                SpawnTile();
                DeleteTile();
            }
        }

        private void SpawnTile()
        {
            GameObject tile = Instantiate(themeManager.GetRandomEnvironmentPrefab(), transform);
            tile.transform.position = Vector3.forward * spawnZ;
            spawnZ += tileLength;
            activeTiles.Add(tile);
        }

        private void DeleteTile()
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
    }
}
