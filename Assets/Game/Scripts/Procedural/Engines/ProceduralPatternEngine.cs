using UnityEngine;
using EndlessRunner.Data;

namespace EndlessRunner.Procedural.Engines
{
    /// <summary>
    /// A sophisticated engine for generating procedural patterns for obstacles and coins.
    /// This is a singleton and should be accessed via its Instance property.
    /// </summary>
    public class ProceduralPatternEngine : MonoBehaviour
    {
        public static ProceduralPatternEngine Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Generates a validated pattern for obstacles and coins based on the provided difficulty and theme.
        /// In a real-world scenario, this would involve a complex set of rules and pre-defined patterns.
        /// For this implementation, we'll use a simple random generator to demonstrate the end-to-end system.
        /// </summary>
        /// <param name="difficultyProfile">The current difficulty profile of the game.</param>
        /// <param name="currentTheme">The current theme, which might influence pattern selection.</param>
        /// <returns>A PatternData object containing the generated layouts.</returns>
        public PatternData GetPattern(DifficultyProfile difficultyProfile, ThemeConfig currentTheme)
        {
            // The number of slots for obstacles and coins is determined by the track segment.
            // For this example, I am assuming a fixed number of 10 slots for each. 
            // A more robust solution would dynamically get this from the segment or theme data.
            int obstacleSlotsCount = 10;
            int coinSlotsCount = 10;

            bool[] obstacleLayout = new bool[obstacleSlotsCount];
            bool[] coinLayout = new bool[coinSlotsCount];

            // Simple random generation based on difficulty.
            // A more advanced implementation would use the difficultyProfile and currentTheme in a more nuanced way.
            float obstacleChance = difficultyProfile.obstacleDensity;
            float coinChance = difficultyProfile.coinFrequency;

            for (int i = 0; i < obstacleSlotsCount; i++)
            {
                obstacleLayout[i] = Random.value < obstacleChance;
            }

            for (int i = 0; i < coinSlotsCount; i++)
            {
                coinLayout[i] = Random.value < coinChance;
            }

            return new PatternData(obstacleLayout, coinLayout);
        }
    }
}
