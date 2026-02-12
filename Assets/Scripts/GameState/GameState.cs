
/// <summary>
/// Defines the possible states of the game application flow.
/// This enum is the single source of truth for determining what the game is currently doing.
/// </summary>
public enum GameState
{
    None,       // The initial state before anything is set up.
    Home,       // The player is in the main menu.
    Playing,    // The player is actively in a run.
    Paused,     // The game is paused.
    GameOver,   // The run has ended, and the summary screen is potentially showing.
    Restarting  // The game is in the process of reloading the scene for a new run.
}
