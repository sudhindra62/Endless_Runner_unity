/// <summary>
/// Enum defining different UI states during gameplay and menu navigation.
/// Used by GameUIManager to track which UI contexts are active.
/// </summary>
public enum UIState
{
    None,
    MainMenu,
    InGame,
    Paused,
    GameOver,
    Shop,
    Settings,
    Achievements,
    Skins,
    BattlePass
}
