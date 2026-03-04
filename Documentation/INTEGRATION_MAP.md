# INTEGRATION_MAP.md: The Cartographer

**ROLE:** A visual and technical map of all system integrations, dependencies, and event chains.
**AUTO-UPDATING LOGIC:** AI Guardian will update this map whenever a new dependency is created or an event link is established.

---

## EVENT & DEPENDENCY MAP

### Player Systems

*   **`PlayerController`** `(class)` -> **`PlayerMovement`**, **`AttackManager`**, **`PlayerCollisionHandler`** `(dependencies)`
    *   **Description:** The core controller relies on these components to manage movement, attacks, and collision responses.
*   **`GameManager.OnGameStateChanged`** `(event)` -> **`PlayerController.OnGameStateChanged`** `(subscriber)`
    *   **Description:** The PlayerController adjusts its behavior based on the global game state.
*   **`PowerUpFusionManager.OnFusionActivated`** `(event)` -> **`PlayerController.HandleFusionActivation`** `(subscriber)`
    *   **Description:** The PlayerController applies fusion-specific effects, like speed boosts.

### Data Persistence (Save/Load)

*   **`SaveManager`** `(class)` -> **`PlayerController`, `InventoryManager`, `MissionManager`** `(runtime dependencies)`
    *   **Description:** The `SaveManager` dynamically finds and references these core managers at runtime to gather the necessary data for creating a save file.

*   **`SaveManager`** `(class)` -> **`ItemDatabase`, `QuestDatabase`** `(runtime dependencies)`
    *   **Description:** The `SaveManager` uses these databases to reconstruct full `Item` and `Quest` objects from the lightweight `ItemData` and `QuestData` stored in the save file.

*   **`SaveManager`** `(class)` -> **`Checksum`** `(dependency)`
    *   **Description:** The `SaveManager` uses a `Checksum` utility to calculate a hash of the save data, ensuring its integrity and detecting any tampering or corruption.

*   **`UI.SaveLoadUI.OnSaveButtonPressed`** `(event)` -> **`SaveManager.SaveGame`** `(subscriber)`
    *   **Description:** A UI button press triggers the central `SaveGame` function.

*   **`UI.SaveLoadUI.OnLoadButtonPressed`** `(event)` -> **`SaveManager.LoadGame`** `(subscriber)`
    *   **Description:** A UI button press triggers the central `LoadGame` function.

### Procedural Pattern Generation

*   **`MasterTileSpawner.OnTileSpawned`** `(event)` -> **`ProceduralPatternEngine.GenerateAndBroadcastPatternForTile`** `(subscriber)`
    *   **Description:** When a new tile is spawned, it triggers the pattern engine to generate obstacles for that tile.

*   **`ProceduralPatternEngine`** `(class)` -> **`PatternSeedManager`** `(dependency)`
    *   **Description:** The engine uses the seed manager to ensure deterministic and replayable pattern generation.

*   **`ProceduralPatternEngine`** `(class)` -> **`SafePathValidator`** `(dependency)`
    *   **Description:** The engine uses the validator to ensure every pattern is fair and has a navigable path.

*   **`ProceduralPatternEngine`** `(class)` -> **`PatternRuleValidator`** `(dependency)`
    *   **Description:** The engine uses the rule validator to ensure the pattern is contextually appropriate for the current game state.

*   **`ProceduralPatternEngine.OnGeneratedPatternReady`** `(event)` -> **`MasterObstacleSpawner.ExecutePatternInstruction`** `(subscriber)`
    *   **Description:** When the engine finalizes a pattern, it broadcasts the instructions to the master spawner for execution.

### UI Systems

*   **`UIManager`** `(class)` -> **`GameHUDController`**, **`HomeScreenController`**, **`PauseMenuController`**, etc. `(dependencies)`
    *   **Description:** The main UI manager controls the state of various UI panels.
*   **`PlayerController.OnPlayerDeath`** `(event)` -> **`UIManager.ShowEndOfRunScreen`** `(subscriber)`
    *   **Description:** The UI Manager displays the end-of-run summary when the player dies.
*   **`ScoreManager.OnScoreChanged`** `(event)` -> **`GameHUDController.UpdateScore`** `(subscriber)`
    *   **Description:** The HUD updates the displayed score in real-time.

### Obstacle Spawning & Effects

*   **`MasterObstacleSpawner`** `(class)` -> **`ObjectPooler`** `(dependency)`
    *   **Description:** The spawner uses an object pooler to efficiently spawn and despawn obstacles without performance overhead.

*   **`MasterObstacleSpawner`** `(class)` -> **`EffectsManager`** `(dependency)`
    *   **Description:** The spawner queries the effects manager to determine if the obstacle-to-coin conversion power-up is active.

*   **`EffectsManager.OnNearMiss`** `(event)` -> **`ProceduralPatternEngine.HandlePlayerNearMiss`** `(subscriber)`
    *   **Description:** When the player has a near miss with an obstacle, the effects manager fires an an event that the pattern engine uses to dynamically increase the obstacle density.
