
using UnityEngine;
using Core;

namespace Core
{
    public class LevelChunk : MonoBehaviour
    {
        private int[] _pattern;

        public void Initialize(int[] pattern)
        {
            _pattern = pattern;
            PopulateChunk();
        }

        private void PopulateChunk()
        {
            if (_pattern == null) return;

            for (int i = 0; i < _pattern.Length; i++)
            {
                switch (_pattern[i])
                {
                    case 1: // Obstacle
                        ObjectPooler.Instance.SpawnFromPool("Obstacle", transform.position + new Vector3(0, 0, i * 2), Quaternion.identity);
                        break;
                    case 2: // Coin
                        ObjectPooler.Instance.SpawnFromPool("Coin", transform.position + new Vector3(0, 1, i * 2), Quaternion.identity);
                        break;
                }
            }
        }
    }
}
