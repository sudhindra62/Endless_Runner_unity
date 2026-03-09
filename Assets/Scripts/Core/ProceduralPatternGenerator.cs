
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Generates procedural patterns for level chunks.
    /// </summary>
    [CreateAssetMenu(fileName = "PatternGenerator", menuName = "Gameplay/Pattern Generator")]
    public class ProceduralPatternGenerator : ScriptableObject
    {
        [Header("Pattern Settings")]
        [SerializeField] private int minLength = 5;
        [SerializeField] private int maxLength = 15;

        /// <summary>
        /// Generates a simple random pattern of integers.
        /// In a real game, these integers would correspond to specific obstacle or collectible prefabs.
        /// 0 = Empty, 1 = Obstacle, 2 = Coin
        /// </summary>
        /// <param name="chunkLength">The desired length of the pattern.</param>
        /// <returns>An array of integers representing the pattern.</returns>
        public int[] GeneratePattern(int chunkLength)
        {
            int[] pattern = new int[chunkLength];
            for (int i = 0; i < chunkLength; i++)
            {
                // Simple random generation for demonstration
                pattern[i] = Random.Range(0, 3); 
            }
            return pattern;
        }
    }
}
