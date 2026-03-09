
using System;

namespace Core
{
    /// <summary>
    /// Defines global game events using a static class for easy access.
    /// This allows for a decoupled communication system between different game components.
    /// </summary>
    public static class GameEvents
    {
        // Called when a new level chunk is generated and ready for population.
        public static Action<LevelChunk> OnLevelChunkGenerated;

        // Called when the player successfully navigates a hazard.
        public static Action OnPlayerSurvivedHazard;

        // Called when the game state changes (e.g., from Menu to Playing).
        public static Action<GameState> OnGameStateChanged;

        // Called when the score is updated.
        public static Action<int> OnScoreUpdated;

        // Called when the player collects a coin.
        public static Action<int> OnCoinCollected;
    }

    /// <summary>
    /// Represents the different states of the game.
    /// </summary>
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }

    /// <summary>
    /// Represents a segment of the level, containing its layout and properties.
    /// </summary>
    public class LevelChunk
    {
        public int Length { get; set; }
        public int[] ObstacleLayout { get; set; }
    }
}
