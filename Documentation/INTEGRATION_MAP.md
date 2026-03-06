# INTEGRATION_MAP.md: The Cartographer

**ROLE:** A visual and technical map of all system integrations, dependencies, and event chains.
**AUTO-UPDATING LOGIC:** AI Guardian will update this map whenever a new dependency is created or an event link is established.

---

## EVENT & DEPENDENCY MAP

### Ghost Run System

*   **`GhostRunRecorder`** `(class)` -> **`Transform`** `(dependency)`
    *   **Description:** The recorder tracks the position of a target `Transform`.
*   **`GhostRunPlayback`** `(class)` -> **`None`**
    *   **Description:** The playback system is self-contained and does not have any hard dependencies.

### Live Events

*   **`EffectsManager.OnNearMiss`** `(event)` -> **`EventProgressTracker.HandleNearMiss`** `(subscriber)`
    *   **Description:** The `EventProgressTracker` listens for near-miss events to update the player's progress in live events.

### Rare Drop & Legendary Shard Engine

*   **`RewardManager.OnRewardCalculation`** `(event)` -> **`RareDropEngine.EvaluateDrop`** `(subscriber)`
    *   **Description:** The RewardManager, as the sole reward authority, triggers the rare drop evaluation *after* standard rewards are calculated but *before* they are displayed.

*   **`RareDropEngine`** `(class)` -> **`RareDropProfileData`, `DropTableRegistry`, `PityCounterManager`, `DropIntegrityValidator`** `(dependencies)`
    *   **Description:** The core engine uses profile data for rarity tiers, a registry for drop tables, a pity manager for bad luck protection, and an integrity validator to prevent exploits.

*   **`DropIntegrityValidator`** `(class)` -> **`RunSessionData`** `(dependency)`
    *   **Description:** The validator cross-references `RunSessionData` against server-side limits to ensure the run was legitimate before allowing a drop.

*   **`PityCounterManager`** `(class)` -> **`SaveManager`** `(dependency)`
    *   **Description:** The `PityCounterManager` interfaces with the `SaveManager` to persist its cross-run pity counters, ensuring bad luck protection works between play sessions.

*   **`ShardInventoryManager`** `(class)` -> **`SaveManager`, `SkinManager`** `(dependencies)`
    *   **Description:** Manages the player's collection of item shards. It uses the `SaveManager` to persist the inventory and calls the `SkinManager` to grant the final item once enough shards are collected.

*   **`RareDropEngine.OnRareDropAwarded`** `(event)` -> **`UI.RareDropUI.AnimateReveal`, `ShardInventoryManager.AddShard`** `(subscribers)`
    *   **Description:** When a rare drop is awarded, the engine broadcasts an event. The UI listens to this to play the reveal animation, and the `ShardInventoryManager` listens to add any awarded shards to the player's inventory.

*   **`LiveEventManager.OnEventBoostsChanged`** `(event)` -> **`RareDropEngine.UpdateMultipliers`** `(subscriber)`
    *   **Description:** The `RareDropEngine` listens for changes from the `LiveEventManager` and `LeagueManager` to apply capped, non-stacking boosts to the drop calculations.

### World Theme Engine

*   **`WorldThemeManager`** `(class)` -> **`ThemeProfileData`**, **`ThemeMaterialRegistry`**, **`ThemeAudioProfile`** `(dependencies)`
    *   **Description:** The manager uses data-driven ScriptableObjects to define and manage all aspects of a visual and audio theme.
*   **`WorldThemeManager.OnThemeApplied`** `(event)` -> **`AudioManager.PlayMusic`** `(subscriber)`
    *   **Description:** The AudioManager plays the appropriate music track defined in the theme's `ThemeAudioProfile`.
*   **`WorldThemeManager.OnThemeApplied`** `(event)` -> **`Various Systems (e.g., MasterObstacleSpawner)`** `(subscriber)`
    *   **Description:** Systems that require themed assets (like materials) subscribe to this event. They then access the `ThemeMaterialRegistry` via the `GetCurrentTheme()` method on the `WorldThemeManager` to get the correct assets.
*   **`LiveEventManager.OnEventStarted`** `(event)` -> **`WorldThemeManager.SetEventThemeOverride`** `(subscriber)`
    *   **Description:** The `LiveEventManager` can remotely trigger a theme change, ensuring the game world reflects the current live event.
*   **`RemoteConfigManager.OnConfigFetched`** `(event)` -> **`WorldThemeManager.SetEventThemeOverride`** `(subscriber)`
    *   **Description:** A manual override from a remote config can be used to force a specific theme, providing ultimate live-ops control.

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
