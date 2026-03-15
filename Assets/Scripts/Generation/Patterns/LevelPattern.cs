
using UnityEngine;

namespace EndlessRunner.Generation.Patterns
{
    // Enum to define what can be placed in the pattern grid
    public enum PatternItemType { Empty, Obstacle, Coin, Wall }

    /// <summary>
    /// A ScriptableObject that defines a reusable, self-contained section of the level.
    /// This is the foundational data structure for the procedural pattern engine.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLevelPattern", menuName = "Endless Runner/Level Pattern")]
    public class LevelPattern : ScriptableObject
    {
        [Header("Pattern Metadata")]
        [Tooltip("A unique identifier for this pattern.")]
        public string patternID;

        [Tooltip("The length of the pattern in grid units.")]
        public int patternLength = 10;

        [Tooltip("The width of the pattern in grid units (typically matches lane count).")]
        public int patternWidth = 3;

        [Tooltip("A subjective difficulty rating, used by the Adaptive AI.")]
        [Range(1, 10)]
        public int difficultyRating = 1;

        [Header("Connection Points")]
        [Tooltip("Defines the state of the lanes at the start of the pattern. True = Open, False = Blocked.")]
        public bool[] entryPoints = new bool[3] { true, true, true };

        [Tooltip("Defines the state of the lanes at the end of the pattern. True = Open, False = Blocked.")]
        public bool[] exitPoints = new bool[3] { true, true, true };

        [Header("Pattern Grid Data")]
        [Tooltip("The core grid defining the layout of items. Serialized for editor persistence.")]
        [SerializeField]
        private PatternItemType[] grid = new PatternItemType[30]; // Default to 10 length * 3 width

        // Provides safe 2D access to the 1D serialized array
        public PatternItemType GetGridItem(int x, int z)
        {
            if (x < 0 || x >= patternWidth || z < 0 || z >= patternLength) return PatternItemType.Wall;
            return grid[z * patternWidth + x];
        }

        // Unity Editor validation
        private void OnValidate()
        {
            if (grid.Length != patternLength * patternWidth)
            {
                System.Array.Resize(ref grid, patternLength * patternWidth);
            }
            if (entryPoints.Length != patternWidth)
            {
                System.Array.Resize(ref entryPoints, patternWidth);
            }
            if (exitPoints.Length != patternWidth)
            {
                System.Array.Resize(ref exitPoints, patternWidth);
            }
        }
    }
}
