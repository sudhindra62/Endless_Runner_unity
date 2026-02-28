# Project Architecture

This document provides a comprehensive and detailed breakdown of all C# script files within the project, organized by their directory structure. The descriptions are intended to be a complete reference, fully explaining the purpose, functionality, methods, and key interactions of each script to enable a deep understanding of the project's architecture without needing to read the source code.

---

## `Assets/Scripts`

### └── `Data`

#### └── `GameState.cs`
*   **Purpose:** Defines the core, type-safe states of the game application.
*   **Role & Functionality:** This script contains a simple but crucial `GameState` enum with values for `Playing`, `Paused`, and `GameOver`. Its role is to provide a standardized, universally accessible set of states that prevent errors from using simple strings or integers. It is primarily used by the `GameStateManager` to set the current state, and by other managers and UI controllers (like `UIManager`) to react to state changes and alter their behavior accordingly. It does not contain any methods or logic, only the enum definition.

#### └── `ObstacleRegistry.cs`
*   **Purpose:** To act as a centralized, high-performance database for all active obstacle GameObjects in the scene.
*   **Role & Functionality:** This is a static class that solves the performance problem of repeatedly searching the scene for obstacles (e.g., with `GameObject.FindObjectsOfType`). It maintains a single static list of all active obstacles.
*   **Static Methods:**
    *   `Register(GameObject obstacle)`: Adds a new obstacle to the internal list. It includes checks to prevent adding null or duplicate entries. This should be called whenever an obstacle is enabled or spawned.
    *   `Unregister(GameObject obstacle)`: Removes an obstacle from the internal list. This should be called whenever an obstacle is disabled or destroyed.
    *   `GetAllActiveObstacles()`: Returns a *new copy* of the list of all currently registered obstacles. A copy is returned to prevent external modification of the internal registry.
    *   `ClearAll()`: Removes all obstacles from the registry. This is essential for resetting the game state at the beginning of a new run.
*   **Interactions:** It is critically used by systems like `SpecialCollectible`'s effect which needs to iterate over and affect all obstacles at once.

---

### └── `Events`

#### └── `GameEvents.cs`
*   **Purpose:** To enable decoupled communication between different game systems using a global event bus.
*   **Role & Functionality:** This static class implements the Observer design pattern. It allows systems to trigger and listen to game-wide events without having direct references to each other, which is key to a clean and maintainable architecture.
*   **Events & Methods:**
    *   `public static event Action<int> OnRunEnded;`: This is the main event. Any script can subscribe to it to be notified when a run ends. It passes the final score as an integer parameter.
    *   `public static void TriggerRunEnded(int finalScore)`: A static wrapper method that safely invokes the `OnRunEnded` event (using the null-conditional operator `?.Invoke`). This is the only way events should be triggered from outside.
*   **Interactions:** `GameFlowController` calls `TriggerRunEnded`. `PlayerDataManager` subscribes to `OnRunEnded` to award XP.

---

### └── `Managers`

#### └── `AdMobManager.cs`
*   **Purpose:** Manages all interactions with the Google Mobile Ads (AdMob) SDK for showing rewarded ads.
*   **Role & Functionality:** This manager abstracts the complexity of the AdMob API into a simple, service-located component.
*   **Initialization:** It initializes the Mobile Ads SDK via `Initialize()` and then calls `RequestRewardedAd()` to load an ad in the background, making it ready to be shown instantly.
*   **Ad Serving:** The public method `ShowRewardedAd(Action onRewardEarnedCallback)` allows any script (like `ReviveManager`) to show an ad. It stores the provided callback to be executed upon success.
*   **Callbacks:** It handles the full event lifecycle of an ad: `HandleUserEarnedReward` invokes the success callback, and `HandleRewardedAdClosed` immediately requests a new ad to be ready for the next opportunity.
*   **Configuration:** It uses the `BuildSettings` service to select between production and test `rewardedAdUnitId`s.

#### └── `BuildSettings.cs`
*   **Purpose:** To provide global, build-specific configuration settings.
*   **Role & Functionality:** As a service-located script, it acts as a centralized source of truth for settings that change between development and production builds.
*   **Properties:** It exposes read-only properties `IsProductionBuild` and `AdsEnabled`.
*   **Interactions:** Other managers, especially `AdMobManager` and `FirebaseManager`, query these properties to alter their behavior (e.g., using test ad units, enabling debug logs) based on the build type.

#### └── `CurrencyManager.cs`
*   **Purpose:** Manages all player currency (coins, gems) transactions and persists the data.
*   **Role & Functionality:** This class is the sole authority for all currency operations, ensuring data integrity. It uses `PlayerPrefs` for persistence.
*   **Methods:** It provides public methods to `AddCoins`, `SpendCoins`, `AddGems`, and `SpendGems`. The spend methods return a boolean indicating success.
*   **Events:** It broadcasts static `OnCoinsChanged` and `OnGemsChanged` events with the new balance whenever a currency amount is updated.
*   **Interactions:** UI binders like `CurrencyUIBinder` subscribe to these events to keep the display updated automatically. Systems like `ReviveManager` call the `SpendGems` method.

#### └── `FirebaseManager.cs`
*   **Purpose:** To initialize and manage all Firebase services used in the game.
*   **Role & Functionality:** This manager handles the setup of Firebase SDKs, ensuring they are configured correctly at the start of the game.
*   **Initialization:** Its `Initialize` method asynchronously checks for `FirebaseApp` dependencies. If successful, it enables `FirebaseAnalytics` data collection and proceeds to `InitializeRemoteConfig`.
*   **Remote Config:** It sets default values and then calls `FetchRemoteConfig` to get the latest configuration from the Firebase backend, activating it upon completion. This is used for remote game tuning and feature flagging.

#### └── `GameManager.cs`
*   **Purpose:** To initialize all core game managers and ensure they persist across scenes.
*   **Role & Functionality:** This script acts as the bootstrap for the entire game. It's a persistent singleton (`DontDestroyOnLoad`) that guarantees all manager systems are available before any other game logic runs.
*   **Methods:** In its `Awake` method, it implements the singleton pattern. `InitializeManagers` then calls the generic helper `InstantiateManager<T>` for each core service (e.g., `PlayerDataManager`, `GameStateManager`). This ensures managers are only instantiated if they don't already exist in the scene.

#### └── `GameStateManager.cs`
*   **Purpose:** Manages the high-level state of the game and controls the game's clock.
*   **Role & Functionality:** This class is the central authority for the game's state. Other systems query `CurrentState` to understand the game context.
*   **Methods:** Its `SetState(GameState newState)` method transitions the game to a new state.
*   **Events:** When the state changes, it fires the `OnGameStateChanged` event.
*   **Interactions:** The `UIManager` subscribes to this event to switch between UI screens. It also directly controls game pause/resume functionality by setting `Time.timeScale` to 0 or 1 based on the state.

#### └── `PlayerDataManager.cs`
*   **Purpose:** Manages player progression data, including level, experience points (XP), and currency.
*   **Role & Functionality:** This manager handles the player's long-term progression, persisting data via `PlayerPrefs`.
*   **XP & Leveling:** It subscribes to `GameEvents.OnRunEnded` to call `AddXPFromRun`, which calculates XP based on score. `CheckForLevelUp` handles the leveling logic.
*   **Events:** It fires `OnLevelChanged` (with the new level) and `OnXPChanged` (with current and next-level XP) to notify the UI of any progression updates.
*   **Data Persistence:** `LoadData` is called on `Awake`, and `SaveData` is called on `OnApplicationQuit` to ensure data is correctly persisted.

#### └── `ReviveManager.cs`
*   **Purpose:** Manages the logic for allowing a player to revive after a run ends.
*   **Role & Functionality:** This class orchestrates the revive flow, ensuring a player can only revive once per run (`hasRevivedThisRun` flag).
*   **Methods:** `InitiateReviveFlow` is called when a run ends. `AttemptGemRevive` tries to spend gems via the `CurrencyManager`. `AttemptAdRevive` calls the `AdMobManager` to show a rewarded ad. `DeclineRevive` ends the flow.
*   **Events:** It communicates the outcome by invoking `OnPlayerRevived` (on success) or `OnReviveDeclined` (on failure or player choice). The `GameFlowController` and `ReviveUIBinder` subscribe to these events.

#### └── `ScoreManager.cs`
*   **Purpose:** Manages the player's score during a run and tracks the all-time high score.
*   **Role & Functionality:** This class is responsible for all score-related logic.
*   **Methods:** Provides `AddPoints` to increment the score during gameplay and `ResetScore` to clear it at the start of a run. `SaveHighScore` is called at the end of a run to check if the `CurrentScore` has surpassed the `HighScore` and saves it to `PlayerPrefs` if so.
*   **Events:** It fires `OnScoreChanged` and `OnHighScoreChanged` events, which the `GameHUDController` and other UI elements subscribe to for display updates.

---

### └── `Powerups`

#### └── `Coin.cs`
*   **Purpose:** Defines the behavior of a single collectible coin.
*   **Role & Functionality:** This script is attached to coin prefabs. It is designed to work with the `ObjectPooler` for efficient reuse.
*   **Methods:** Its `Collect` method is the primary entry point, called by the `PlayerController`. This method adds its `value` to the `CurrencyManager`, accounting for the `CoinDoubler`'s active state. It then deactivates itself and returns to the object pool.
*   **State:** It maintains `isAttracted` state, which is set by the `CoinMagnet` to prevent multiple attraction coroutines.

#### └── `CoinDoubler.cs`
*   **Purpose:** Manages the global state of the "coin doubler" power-up.
*   **Role & Functionality:** This singleton provides a global point of access to check if the coin doubler is active.
*   **Properties & Methods:** It has an `IsDoublerActive` property that other scripts (primarily `Coin.cs`) can check. `ActivateDoubler` and `DeactivateDoubler` methods control this state.

#### └── `CoinMagnet.cs`
*   **Purpose:** Implements the coin magnet power-up, which attracts nearby coins to the player.
*   **Role & Functionality:** Attached to the player, this script manages its own lifecycle using coroutines.
*   **Methods:** `ActivateMagnet` starts the main `MagnetRoutine` coroutine, which runs for the `magnetDuration`. This routine periodically calls `AttractNearbyCoins`, which uses a non-allocating physics overlap check (`Physics.OverlapSphereNonAlloc`) to find coin colliders. For each new coin found, it starts an `AttractCoin` coroutine to move that coin towards the player until collected.

#### └── `SpecialCollectible.cs`
*   **Purpose:** Represents a special collectible that triggers an immediate, powerful in-game effect.
*   **Role & Functionality:** When collected by the player, this script executes a unique gameplay effect.
*   **Methods:** Its `Collect` method finds the `ObstacleConversionEffect` in the scene and calls its `ConvertAllObstacles` method. It then returns itself to the `ObjectPooler` using its `poolTag`.

---

### └── `Systems`

#### └── `GameFlowController.cs`
*   **Purpose:** Orchestrates the primary game loop by coordinating between various game systems.
*   **Role & Functionality:** This is a crucial controller that connects many managers. It's responsible for starting, pausing, resuming, and ending a run.
*   **Methods:** `StartRun` resets the `ScoreManager`, `ReviveManager`, `PlayerController`, and spawners. `TogglePause`, `Pause`, and `Resume` methods control the game state via the `GameStateManager`. `EndRun` saves the score, triggers the `OnRunEnded` event, and initiates the `ReviveManager`'s flow. `EndRunFinal` is called after the revive flow is complete to transition to the final game over state.
*   **Event Handling:** It subscribes to the `ReviveManager`'s `OnPlayerRevived` and `OnReviveDeclined` events to either resume the game (`OnPlayerRevived`) or finalize the game over state (`OnReviveDeclined` calls `EndRunFinal`).

#### └── `NewCollectibleSpawner.cs`
*   **Purpose:** Manages the procedural spawning of collectible items in groups ahead of the player.
*   **Role & Functionality:** This system populates the game world with coins and other collectibles, using the `ObjectPooler` for efficiency.
*   **Methods:** Its `Update` method checks if the player has moved far enough to trigger `SpawnCollectibleGroups`. This method then iterates through its configured `collectibleGroups`, and based on `spawnChance`, places a group of collectibles in a random lane.
*   **Safety Check:** Before placing an object, `IsPositionSafe` is called, which uses `Physics.CheckBox` to ensure the spawn position is not overlapping with anything on the "Obstacle" layer.

#### └── `ObjectPooler.cs`
*   **Purpose:** Provides a generic, reusable object pooling system to optimize performance by reducing instantiation/destruction overhead.
*   **Role & Functionality:** A fundamental, persistent singleton optimization system.
*   **Initialization:** On `Awake`, it creates pools for each `ObjectPoolItem` defined in its configuration, pre-instantiating the specified `amountToPool` of each object.
*   **Methods:** `GetPooledObject(string tag)` retrieves an inactive object from the specified pool (and expands the pool if `shouldExpand` is true and it's empty). `ReturnToPool(string tag, GameObject objectToReturn)` deactivates an object and returns it to its pool.

#### └── `PlayStoreReadiness.cs`
*   **Purpose:** To integrate with the Google Play Store for in-app reviews.
*   **Role & Functionality:** This script handles the logic for prompting the user for a store review at an appropriate moment.
*   **Methods:** It uses coroutines to manage the asynchronous flow. `RequestReviewInfo` calls the `ReviewManager.RequestReviewFlow` API. If successful, `LaunchReviewFlow` is called to use the resulting `PlayReviewInfo` object to launch the review dialog. The actual display is controlled by the Google Play API.

#### └── `PlayerController.cs`
*   **Purpose:** To handle all player movement, input processing, and physical interactions with the game world.
*   **Role & Functionality:** The core of the player's physical presence in the game.
*   **Movement:** Uses a `Rigidbody` for constant forward `moveSpeed`. `HandleLaneChanging` uses `Vector3.Lerp` for smooth side-to-side movement.
*   **Input:** `HandleInput` detects swipe gestures (horizontal for lane changes, vertical for jumping/sliding).
*   **Actions:** `HandleJumping` applies a `jumpForce` impulse. `HandleSliding` starts a coroutine to temporarily change the capsule collider's height and center.
*   **Collisions:** `OnTriggerEnter` collects `Coin` objects. `OnCollisionEnter` detects obstacles and fires the `OnPlayerHitObstacle` event, which `GameFlowController` uses to end the run.
*   **State:** `ResetPlayerToStart` and `Revive` methods reset the player's state at the start of a run or after reviving.

#### └── `SceneLoader.cs`
*   **Purpose:** To provide a simple, centralized, and decoupled way to load scenes.
*   **Role & Functionality:** This script abstracts Unity's `SceneManager` into a service.
*   **Methods:** It has a single public method, `LoadScene(string sceneName)`, which calls `SceneManager.LoadScene`.
*   **Interactions:** Because it is registered with the `ServiceLocator`, any script can request a scene change via `ServiceLocator.Get<SceneLoader>().LoadScene(...)` without needing a direct dependency on the `SceneManager` API.

#### └── `ServiceLocator.cs`
*   **Purpose:** Implements the Service Locator design pattern to provide a centralized access point for game-wide services.
*   **Role & Functionality:** A critical architectural script that decouples service consumers from providers.
*   **Methods:** It has three static methods: `Register<T>(T service)` to add a service, `Unregister<T>()` to remove it, and `Get<T>()` to retrieve a service by its type. This prevents the need for complex singleton references and dependencies between managers.

#### └── `TileSpawner.cs`
*   **Purpose:** To procedurally generate the endless running track, creating the illusion of an infinite world.
*   **Role & Functionality:** Manages the lifecycle of environment track segments (tiles).
*   **Methods:** In `Update`, it checks if the player has moved far enough to require a new tile. If so, it calls `SpawnTile` to instantiate a new tile prefab from its array and `DeleteTile` to destroy the oldest tile, keeping the number of active tiles constant and the scene performant.

---

### └── `UI`

#### └── `AnimatedButton.cs`
*   **Purpose:** Provides simple, non-intrusive visual feedback for button presses.
*   **Role & Functionality:** A utility script that implements `IPointerDownHandler` and `IPointerUpHandler`. The `OnPointerDown` method scales the button's transform down slightly, and `OnPointerUp` scales it back to its original size, making the UI feel more tactile and responsive.

#### └── `CharacterArtDisplay.cs`
*   **Purpose:** To display character artwork in a UI `Image` component.
*   **Role & Functionality:** Likely used in a character selection or profile screen. Its single public method `SetCharacterArt(Sprite art)` takes a `Sprite` and applies it to its serialized `characterArtImage` field.

#### └── `DailyMissionCardController.cs`
*   **Purpose:** Controls the UI for a single daily mission entry in a list.
*   **Role & Functionality:** Attached to a mission card prefab. Its `Setup(string name, string description, string reward, float progress)` method populates the card's UI elements (`Text` and `Slider`) with the data for a specific mission.

#### └── `DailyMissionPanelUI.cs`
*   **Purpose:** Manages the UI panel that displays a list of daily missions.
*   **Role & Functionality:** This script acts as a container and controller. Its `AddMissionCard(...)` method instantiates a `missionCardPrefab` inside its `missionListContainer` and then uses `GetComponent<DailyMissionCardController>().Setup(...)` to populate it with data.

#### └── `DailyMissionUI.cs`
*   **Purpose:** Manages the UI for a single daily mission.
*   **Role & Functionality:** This script controls the display of a single mission's name and progress bar. Its `SetMissionData(string name, float progress)` method updates the mission's name text and the value of its progress slider.

#### └── `DailyRewardView.cs`
*   **Purpose:** Manages the visibility of the daily reward UI screen.
*   **Role & Functionality:** This script provides a simple interface to show or hide the daily reward panel. Its `Show()` and `Hide()` methods toggle the `active` state of the `GameObject` this script is attached to.

#### └── `FeatureVisibilityToggle.cs`
*   **Purpose:** To show or hide a UI element based on a feature flag, for A/B testing or phased rollouts.
*   **Role & Functionality:** In its `Start` method, it checks a feature flag (using `PlayerPrefs` in this example) based on the provided `featureName` and calls `SetActive` on the `targetGameObject` based on the result.

#### └── `GameHUDController.cs`
*   **Purpose:** Controls the main in-game Head-Up Display (HUD).
*   **Role & Functionality:** Responsible for managing UI elements visible during gameplay. It subscribes its `UpdateScore(int score)` method to the `ScoreManager.OnScoreChanged` event, ensuring the score `Text` component is automatically updated whenever the score changes.

#### └── `HomeNavigationUI.cs`
*   **Purpose:** Handles the main navigation buttons on the home screen.
*   **Role & Functionality:** Wires up the primary buttons on the main menu. In its `Start` method, it adds listeners to the `onClick` events of the `shopButton`, `missionsButton`, and `playButton`, calling the appropriate navigation or scene-loading methods (`GoToShop`, `GoToMissions`, `PlayGame`).

#### └── `HomeScreenController.cs`
*   **Purpose:** Controls the overall behavior and primary action of the home screen.
*   **Role & Functionality:** The main controller for the home screen UI. Its primary role is to handle the `playButton`'s `onClick` event, which calls `StartGame()` to use the `SceneLoader` service to load the main game scene.

#### └── `HomeScreenStyler.cs`
*   **Purpose:** To apply a consistent visual style or theme to the home screen.
*   **Role & Functionality:** Allows for centralized control over the home screen's appearance. Its `ApplyStyle()` method contains logic to set the `backgroundColor` and `headerTextColor` of various UI elements.

#### └── `LevelBadgeUI.cs`
*   **Purpose:** To display the player's current progression level in a dedicated UI badge.
*   **Role & Functionality:** A small, focused UI controller that subscribes its `UpdateLevel(int level)` method to the `PlayerDataManager.OnLevelChanged` event, automatically updating its `levelText` component whenever the player levels up.

#### └── `MagnetTier.cs`
*   **Purpose:** Defines the data structure for a single upgrade tier of the magnet power-up.
*   **Role & Functionality:** This is a simple, serializable data-holding class (not a `MonoBehaviour`). Each instance holds the public `duration` and `cost` fields for a specific tier, allowing the upgrade path to be easily configured in the Unity Inspector as an array of `MagnetTier` objects.

#### └── `MagnetUIController.cs`
*   **Purpose:** Manages the UI for displaying and upgrading the magnet power-up in the shop.
*   **Role & Functionality:** Displays the current tier information and handles the `upgradeButton`'s `onClick` event. The `Upgrade()` method would contain the logic to interact with the `CurrencyManager` to process the transaction and a data manager to save the new tier level.

#### └── `MilestoneUI.cs`
*   **Purpose:** Controls the UI for a single long-term progression milestone or achievement.
*   **Role & Functionality:** Visualizes the player's progress towards a specific goal. Its `SetMilestoneData(string name, float progress)` method updates a `Text` component with the milestone's name and a `Slider` to show the completion progress.

#### └── `MissionUI.cs`
*   **Purpose:** Manages a UI panel that displays a list of missions or milestones.
*   **Role & Functionality:** Acts as a container. Its `AddMission(string name, float progress)` method instantiates a mission prefab and uses its `MilestoneUI` component to populate it with the relevant data.

#### └── `PauseButtonUI.cs`
*   **Purpose:** Handles the functionality of the in-game pause button.
*   **Role & Functionality:** A simple script that connects the UI to the game logic. In `Start`, it adds a listener to the `pauseButton`'s `onClick` event that calls `GameFlowController.TogglePause()`.

#### └── `PlayerStatusNotifier.cs`
*   **Purpose:** To display short, animated status notifications to the player (e.g., 'Power-up Activated!').
*   **Role & Functionality:** The `ShowNotification(string message)` method sets the message on a `Text` component and then triggers a "Show" animation on its `Animator` to make the notification appear and fade out.

#### └── `PowerupTimerUI.cs`
*   **Purpose:** Displays a countdown timer for an active power-up.
*   **Role & Functionality:** Provides visual feedback for the remaining duration of a power-up. Its `SetTime(float time)` method takes a float and updates a `Text` component, formatting it to one decimal place.

#### └── `RankUI.cs`
*   **Purpose:** To display the player's rank on a leaderboard or results screen.
*   **Role & Functionality:** Its `SetRank(int rank)` method takes an integer and updates a `Text` component, formatting it with a "#" prefix.

#### └── `ReviveUI.cs`
*   **Purpose:** Manages the user interaction for the revive screen shown on player death.
*   **Role & Functionality:** The view controller for the revive panel. It wires up `onClick` listeners for `reviveButton`, `reviveWithAdButton`, and `noThanksButton`, calling the corresponding methods on the `ReviveManager` service (`AttemptGemRevive`, `AttemptAdRevive`, `DeclineRevive`). After a choice is made or the revive fails, it calls `GameFlowController.Instance.EndRunFinal()` to ensure the game transitions to the final game over state.

#### └── `RewardChestUI.cs`
*   **Purpose:** Controls the UI and animation for an interactive reward chest.
*   **Role & Functionality:** Listens for a click on the `chestButton`. When clicked, its `OpenChest()` method triggers an "Open" animation on its `Animator` component to provide visual feedback of opening the chest.

#### └── `ScoreMultiplierUI.cs`
*   **Purpose:** Displays the current score multiplier to the player.
*   **Role & Functionality:** Gives the player feedback on their scoring potential. Its `SetMultiplier(int multiplier)` method updates a `Text` component with the current multiplier value, formatted with an "x" prefix.

#### └── `ShieldIconUI.cs`
*   **Purpose:** To show or hide the shield power-up icon, indicating if the power-up is active.
*   **Role & Functionality:** Provides a visual indicator to the player. Its `SetShieldActive(bool active)` method enables or disables the `Image` component of the shield icon.

#### └── `ShopProductCard.cs`
*   **Purpose:** Controls the UI for a single item for sale in the shop.
*   **Role & Functionality:** Attached to a product card prefab. Its `Setup(string name, string description, string price)` method populates the product's `productNameText`, `productDescriptionText`, and `priceText` UI elements.

#### └── `ShopViewController.cs`
*   **Purpose:** Manages the main view of the in-game shop.
*   **Role & Functionality:** The controller for the shop screen. Its `AddProductCard(...)` method populates the view by instantiating `productCardPrefab` objects inside its `productListContainer` and calling their `Setup` method.

#### └── `StyleUI.cs`
*   **Purpose:** To apply a generic visual style to a UI element, as part of a theming system.
*   **Role & Functionality:** Its `ApplyStyle()` method contains logic to set the `backgroundColor` and `textColor` of the `Image` and `Text` components on the `GameObject` it is attached to.

#### └── `UIManager.cs`
*   **Purpose:** Manages the visibility of high-level UI screens, acting as a UI state machine.
*   **Role & Functionality:** Subscribes its `OnGameStateChanged(GameState newState)` method to the `GameStateManager`. When the game state changes, this method deactivates all screens and then activates the single screen that corresponds to the new state (e.g., showing `homeScreen` for `GameState.MainMenu`).

#### └── `XPBarUI.cs`
*   **Purpose:** Displays the player's XP progression towards the next level.
*   **Role & Functionality:** Provides a visual representation of long-term progress. It subscribes its `UpdateXP(int currentXP, int xpForNextLevel)` method to the `PlayerDataManager.OnXPChanged` event, updating both a `Slider`'s value and a `Text` component to reflect the player's progress.

---

### └── `UI/Bindings`

#### └── `CurrencyUIBinder.cs`
*   **Purpose:** To automatically keep the currency display UI synchronized with the `CurrencyManager`.
*   **Role & Functionality:** A binder script that follows the Model-View-Controller pattern. It subscribes `UpdateCoinBalance` and `UpdateGemBalance` to the `CurrencyManager`'s `OnCoinsChanged` and `OnGemsChanged` events, respectively. This ensures the UI text is always up-to-date without other scripts needing to manually refresh it.

#### └── `ReviveUIBinder.cs`
*   **Purpose:** To control the visibility of the revive panel by binding it to events from the `ReviveManager`.
*   **Role & Functionality:** This script shows and hides the revive panel. It has a public `ShowRevivePanel` method, which is called by the `GameFlowController` when a run ends. It automatically hides the panel by subscribing to the `OnPlayerRevived` and `OnReviveDeclined` events from the `ReviveManager`.

---

## Recent Refactoring

This section summarizes recent refactoring efforts to improve the project's architecture, performance, and maintainability.

### Code Cleanup and Optimization

*   **Service Locator Integration:** Replaced a direct call to `Object.FindObjectOfType` in `Assets/Scripts/Gameplay/StyleBonusCalculator.cs` with the `ServiceLocator`. This change enhances performance by avoiding expensive scene-wide searches and adheres to the project's dependency management strategy.

### Project Organization

*   **Duplicate Script Removal:** To resolve compilation errors and streamline the project structure, the following duplicate scripts were removed:
    *   `ScoreInterceptor.cs`
    *   `StyleBonusCalculator.cs`
    *   `StyleMeter.cs`
    *   `PerfectDodgeDetector.cs`
    *   `ShopManager.cs`
    *   `UnlockType.cs`
*   **Orphaned File Deletion:** Removed multiple `.meta` files that were no longer linked to any assets, cleaning up the project repository.
