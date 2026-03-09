
using UnityEngine;

namespace Core
{
    public class ProceduralPatternGenerator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PatternDifficultyProfile difficultyProfile;
        [SerializeField] private SafePathValidator safePathValidator;

        private float _gameTime => Time.time; // Simplified game time access

        /// <summary>
        /// Generates a valid and playable obstacle pattern for a level chunk.
        /// </summary>
        public int[] GeneratePattern(int chunkLength)
        {
            int[] pattern = new int[chunkLength];
            float density = difficultyProfile.GetCurrentDensity(_gameTime);
            float complexChance = difficultyProfile.GetComplexObstacleChance(_gameTime);
            int maxIterations = 20; // Safety break to prevent infinite loops

            for (int i = 0; i < maxIterations; i++)
            {
                pattern = GenerateSinglePattern(chunkLength, density, complexChance);

                if (safePathValidator.IsPathSafe(pattern, chunkLength))
                {
                    return pattern;
                }
            }
            
            Debug.LogWarning("Could not generate a safe path after multiple iterations. Returning a sparse pattern.");
            return GenerateSparsePattern(chunkLength); // Fallback to a guaranteed safe pattern
        }

        /// <summary>
        /// Generates a single pattern iteration without safety checks.
        /// </summary>
        private int[] GenerateSinglePattern(int length, float density, float complexChance)
        {
            int[] pattern = new int[length];
            int activeLanes = safePathValidator.NumberOfLanes;

            for (int i = 0; i < length; i++)
            {
                pattern[i] = 0; // Initialize with no obstacles
                if (Random.value < density)
                {
                    int laneToBlock = Random.Range(0, activeLanes);
                    pattern[i] |= (1 << laneToBlock);

                    if (activeLanes > 1 && Random.value < complexChance)
                    {
                        int secondLaneToBlock = (laneToBlock + Random.Range(1, activeLanes)) % activeLanes;
                        pattern[i] |= (1 << secondLaneToBlock);
                    }
                }
            }
            return pattern;
        }

        /// <summary>
        /// Generates a simple, sparse pattern as a fallback.
        /// </summary>
        private int[] GenerateSparsePattern(int length)
        { 
            int[] pattern = new int[length];
            int lastBlockedLane = -1;
            int activeLanes = safePathValidator.NumberOfLanes;

            for (int i = 0; i < length; i++)
            {
                if (i % 4 == 0) // Place an obstacle every 4 units
                {
                    int laneToBlock = Random.Range(0, activeLanes);
                    // Ensure the new obstacle doesn't block the same lane as the last one
                    if (laneToBlock == lastBlockedLane)
                    {
                        laneToBlock = (laneToBlock + 1) % activeLanes;
                    }
                    pattern[i] = (1 << laneToBlock);
                    lastBlockedLane = laneToBlock;
                }
                else
                {
                    pattern[i] = 0;
                }
            }
            return pattern;
        }
    }
}
