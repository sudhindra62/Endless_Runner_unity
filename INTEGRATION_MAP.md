# INTEGRATION_MAP.md: The Technical Architect

**ROLE:** Maps the communication pathways and dependencies between all major scripts and systems.
**AUTO-UPDATING LOGIC:** The AI Guardian scans for event subscriptions (`+=`), `GetComponent` calls, and direct script references to keep this map accurate.

---

### **[A] Core Gameplay Loop & System Events**

This section details the primary event chains that drive the game.

**1. Game State Control:**
-   `[GameManager] --(OnGameStateChanged)--> [UIManager]`
    -   **Effect:** `UIManager` activates/deactivates UI panels (Menu, Gameplay, Pause, etc.).
-   `[GameManager] --(OnGameStateChanged)--> [ScoreManager]`
    -   **Effect:** `ScoreManager` resets the score when the state changes to `EndOfRun`.

**2. Tile Spawning & Obstacle Generation**
-   `[MasterTileSpawner] --(OnTileSpawned)--> [ProceduralPatternEngine]`
    -   **Effect:** `ProceduralPatternEngine` begins the pattern generation process for the new tile.
-   `[ProceduralPatternEngine] --(OnGeneratedPatternReady)--> [MasterObstacleSpawner]`
    -   **Effect:** `MasterObstacleSpawner` physically spawns the generated obstacle pattern.

**3. Player Input & State:**
-   `[SwipeInput] --(Direct Call)--> [PlayerMovement]`
    -   **Effect:** Translates player swipes into lane changes, jumps, and slides.
-   `[PlayerCollision] --(OnPlayerDeath)--> [GameManager]`
    -   **Effect:** Reports player death, triggering the `EndOfRun` game state.
-   `[PlayerCollision] --(OnPlayerDeath)--> [AnalyticsManager]`
    -   **Effect:** Logs the `player_death` event.
-   `[PlayerCollision] --(OnPlayerDeath)--> [UIManager]`
    -   **Effect:** Displays the end-of-run screen.


**4. Scoring & UI Feedback:**
-   `[ScoreManager] --(OnScoreChanged)--> [UIManager]`
    -   **Effect:** `UIManager` updates the `scoreText` in the gameplay HUD.

**5. Power-Up & Fusion Effects:**
-   `[PowerUpCollector] --(OnPowerUpCollected)--> [PowerUpManager]`
    -   **Effect:** Activates the corresponding power-up.
-   `[PowerUpFusionManager] --(OnFusionActivated/Deactivated)--> [UIManager]`
    -   **Effect:** `UIManager` shows/hides the `FusionUI`.
-   `[PowerUpFusionManager] --(Direct Call)--> [PlayerController]`
    -   **Effect:** Activates/deactivates player's shield or fever mode.
-   `[PowerUpFusionManager] --(Direct Call)--> [ScoreManager]`
    -   **Effect:** Applies a score multiplier.

**6. Currency & UI:**
-   `[CurrencyManager] --(OnCurrencyChanged)--> [UIManager]`
    -   **Effect:** Updates Coin/Gem displays.

**7. Ad-Based Revive:**
-   `[ReviveManager] --(RequestRewardedAd)--> [AdManager]` and `[AdManager] --(OnAdCompleted)--> [ReviveManager]`

**8. Data Persistence:**
-   `[SaveManager] --(Load/Save)--> [CurrencyManager, MissionManager, SkinsManager]`
    -   **Effect:** Persists player data.

**9. Mission Progression & UI:**
-   `[MissionManager] --(OnMissionProgress)--> [UIManager]`
    -   **Effect:** Displays quest status pop-ups.

---

### **[B] System Gaps & Planned Integrations**

This section lists critical connections that are designed but not yet implemented.

-   **[PLANNED] Remote Config Balancing:**
    -   **GAP:** `RemoteConfigBridge` is not implemented.
    -   **INTEGRATION:** `[GameManager, PlayerMovement, etc.] --(FetchValue)--> [RemoteConfigBridge]` to pull balance values from the `GAME_DASHBOARD.md` via Firebase.
