
using UnityEngine;
using System.Collections.Generic;

namespace EndlessRunner.Themes
{
    public class EnvironmentVariationManager : MonoBehaviour
    {
        public static EnvironmentVariationManager Instance { get; private set; }

        private void Awake()
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

        public void SpawnInitialEnvironment()
        {
            ThemeData theme = ThemeManager.Instance.GetCurrentTheme();
            if (theme == null || theme.environmentPrefabs.Length == 0) return;

            // Spawn initial set of environment pieces
            for (int i = 0; i < 5; i++) // for example, 5 initial pieces
            {
                SpawnRandomEnvironmentSegment(i * 20.0f); // Assuming each segment is 20 units long
            }
        }

        public void SpawnRandomEnvironmentSegment(float zPos)
        {
            ThemeData theme = ThemeManager.Instance.GetCurrentTheme();
            if (theme == null || theme.environmentPrefabs.Length == 0) return;

            int randomIndex = Random.Range(0, theme.environmentPrefabs.Length);
            GameObject prefab = theme.environmentPrefabs[randomIndex];

            Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity, transform);
        }
    }
}

