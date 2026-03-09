
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Validates obstacle patterns to ensure they are physically navigable by the player.
    /// This script acts as a "fairness" check to prevent unbeatable level segments.
    /// </summary>
    public class SafePathValidator : MonoBehaviour
    {
        [Header("Player Movement Parameters")]
        [Tooltip("The number of available lanes for the player.")]
        [SerializeField] private int numberOfLanes = 3;

        [Tooltip("The time required for the player to switch from one lane to another.")]
        [SerializeField] private float laneSwitchTime = 0.2f;

        public int NumberOfLanes => numberOfLanes;

        /// <summary>
        /// Analyzes a generated pattern to determine if a safe path exists for the player.
        /// </summary>
        /// <param name="pattern">The obstacle pattern, represented as an array of bitmasks.</param>
        /// <param name="chunkLength">The length of the pattern array.</param>
        /// <returns>True if a safe path is found, false otherwise.</returns>
        public bool IsPathSafe(int[] pattern, int chunkLength)
        {
            // This simplified validator checks for two main conditions:
            // 1. That there is never a full blockage of all lanes at the same time.
            // 2. That there is always at least one open lane to move into.

            int allLanesBlockedMask = (1 << numberOfLanes) - 1;

            for (int i = 0; i < chunkLength; i++)
            {
                // Check for a full blockage
                if (pattern[i] == allLanesBlockedMask)
                {
                    return false; // Instant failure if all lanes are blocked
                }

                // Check if a lane switch is required and possible
                if (i > 0 && pattern[i-1] != 0 && pattern[i] != 0)
                {
                    int previousOpenLanes = allLanesBlockedMask & ~pattern[i-1];
                    int currentOpenLanes = allLanesBlockedMask & ~pattern[i];
                    
                    // If there are no common open lanes, a switch is needed.
                    if ((previousOpenLanes & currentOpenLanes) == 0)
                    {
                        // This simplistic check doesn't account for the *time* to switch,
                        // but it ensures there is at least a theoretical path.
                        // A more advanced validator would use gameSpeed and chunkDistance.
                        bool canSwitch = false;
                        for(int prevLane = 0; prevLane < numberOfLanes; prevLane++)
                        {
                            if(((previousOpenLanes >> prevLane) & 1) == 1)
                            {
                                for(int currLane = 0; currLane < numberOfLanes; currLane++)
                                {
                                    if((currentOpenLanes >> currLane) & 1) == 1
                                    {
                                        // In a real scenario, you'd calculate if the distance between obstacles
                                        // is greater than player speed * laneSwitchTime.
                                        canSwitch = true; 
                                        break;
                                    }
                                }
                            }
                            if(canSwitch) break;
                        }

                        if (!canSwitch)
                        {
                            return false;
                        }
                    }
                }
            }

            return true; // The pattern is considered safe
        }
    }
}
