using UnityEngine;

namespace EndlessRunner.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public static LevelGenerator Instance { get; private set; }

        public float speed = 10f;
        public float speedIncreaseRate = 0.1f;

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

        private void Update()
        {
            // Increase speed over time
            speed += speedIncreaseRate * Time.deltaTime;
        }

        public GameObject GetThemedSegment()
        {
            if (ThemeManager.Instance != null && ThemeManager.Instance.currentTheme != null)
            {
                var theme = ThemeManager.Instance.currentTheme;
                var segmentPrefab = theme.segmentPrefabs[Random.Range(0, theme.segmentPrefabs.Length)];
                var segmentObject = SegmentPoolManager.Instance.GetSegment(segmentPrefab);
                if (segmentObject != null)
                {
                    var segment = segmentObject.GetComponent<TrackSegment>();
                    if (segment != null)
                    {
                        segment.prefab = segmentPrefab;
                    }
                }
                return segmentObject;
            }
            return null;
        }
    }
}
