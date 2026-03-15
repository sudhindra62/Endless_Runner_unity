
using EndlessRunner.Generation.Patterns;

namespace EndlessRunner.Generation.Rules
{
    /// <summary>
    /// A static utility class that contains rules for connecting level patterns.
    /// Ensures that the generated level path is always traversable.
    /// </summary>
    public static class PatternRuleValidator
    {
        /// <summary>
        /// Validates if two level patterns can be connected based on their entry and exit points.
        /// </summary>
        /// <param name="fromPattern">The pattern that is already placed.</param>
        /// <param name="toPattern">The candidate pattern to be placed next.</param>
        /// <returns>True if a valid, traversable path exists between the patterns.</returns>
        public static bool CanConnect(LevelPattern fromPattern, LevelPattern toPattern)
        {
            // The number of lanes must match.
            if (fromPattern.exitPoints.Length != toPattern.entryPoints.Length)
            {
                return false;
            }

            // There must be at least one open lane that connects the two patterns.
            for (int i = 0; i < fromPattern.exitPoints.Length; i++)
            {
                // If an exit lane is open and its corresponding entry lane is also open, a path exists.
                if (fromPattern.exitPoints[i] && toPattern.entryPoints[i])
                {
                    return true;
                }
            }

            // If we loop through all lanes and find no connecting open path, the connection is invalid.
            return false;
        }
    }
}
