# INTEGRATION_MAP.md: The Technical Architect

**ROLE:** Maps the communication pathways and dependencies between all major scripts and systems.
**AUTO-UPDATING LOGIC:** The AI Guardian scans for event subscriptions (`+=`), `GetComponent` calls, and direct script references to keep this map accurate.

---

### **[A] Core Gameplay Loop Data Flow**

This section details the primary, implemented event chain that drives the game.

1.  **Game State Control:**
    -   `[GameManager] --(OnGameStateChanged)--> [UIManager]`
        -   **Effect:** `UIManager` activates/deactivates UI panels (Menu, Gameplay, Pause, etc.).
    -   `[GameManager] --(OnGameStateChanged)--> [ScoreManager]`
        -   **Effect:** `ScoreManager` resets the score when the state changes to `EndOfRun`.

2.  **Scoring & UI Feedback:**
    -   `[ScoreManager] --(OnScoreChanged)--> [UIManager]`
        -   **Effect:** `UIManager` updates the `scoreText` TMP element in the gameplay HUD.

3.  **Power-Up & Fusion Effects:**
    -   `[PowerUpFusionManager] --(OnFusionActivated)--> [UIManager]`
        -   **Effect:** `UIManager` receives `FusionModifierData` and calls `ShowFusionUI()`.
    -   `[PowerUpFusionManager] --(OnFusionDeactivated)--> [UIManager]`
        -   **Effect:** `UIManager` calls `HideFusionUI()`.
    -   `[PowerUpFusionManager] --(Direct Call)--> [PlayerController]`
        -   **Effect:** Activates/deactivates the player's shield (`SetShield(bool)`) or fever mode.
    -   `[PowerUpFusionManager] --(Direct Call)--> [ScoreManager]`
        -   **Effect:** Applies a score multiplier (`SetScoreMultiplier(float)`).

4.  **Player Input & State:**
    -   `[SwipeInput] --(Direct Call)--> [PlayerMovement]`
        -   **Effect:** Translates player swipes into lane changes, jumps, and slides.
    -   `[PlayerController] --(Direct Call)--> [GameManager]`
        -   **Effect:** Reports player death, triggering the `Dead` game state.

5.  **Currency & UI:**
    -   `[CurrencyManager] --(OnCurrencyChanged)--> [UIManager]`
        -   **Effect:** Updates Coin/Gem displays.

6.  **Ad-Based Revive:**
    -   `[ReviveManager] --(RequestRewardedAd)--> [AdManager]` and `[AdManager] --(OnAdCompleted)--> [ReviveManager]`

7.  **Data Persistence:**
    -   `[SaveManager] --(Load/Save)--> [CurrencyManager, MissionManager, SkinsManager]`
        -   **Effect:** Persists player data.

8.  **Mission Progression & UI:**
    -   `[MissionManager] --(OnMissionProgress)--> [UIManager]`
        -   **Effect:** Displays quest status pop-ups.

---

### **[B] System Gaps & Planned Integrations**

This section lists critical connections that are designed but not yet implemented, as per the `MASTER_FEATURE_REGISTRY.md`.

-   **[PLANNED] Remote Config Balancing:**
    -   **GAP:** `RemoteConfigBridge` is not implemented.
    -   **INTEGRATION:** `[GameManager, PlayerMovement, etc.] --(FetchValue)--> [RemoteConfigBridge]` to pull balance values from the `GAME_DASHBOARD.md` via Firebase.
