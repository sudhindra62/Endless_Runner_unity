
using UnityEngine;

namespace EndlessRunner.Generation
{
    public static class ProceduralPatternGenerator
    {
        // A simple example of procedural generation.
        // A real implementation would be much more complex.
        public static int[] GeneratePattern(int length)
        {
            int[] pattern = new int[length];

            for (int i = 0; i < length; i++)
            {
                float roll = Random.Range(0f, 1f);

                if (roll < 0.2f) // 20% chance of an obstacle
                {
                    pattern[i] = 1;
                }
                else if (roll < 0.5f) // 30% chance of a coin
                {
                    pattern[i] = 2;
                }
                else // 50% chance of being empty
                {
                    pattern[i] = 0;
                }
            }
            return pattern;
        }
    }
}
