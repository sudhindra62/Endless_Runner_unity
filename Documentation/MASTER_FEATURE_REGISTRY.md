# MASTER_FEATURE_REGISTRY.md: The Project Dictionary

**PROTOCOL:** This file is the definitive, auto-updating dictionary of every feature in the project. It is maintained by the AI Guardian and serves as the primary source of truth for system architecture and integration status.

---

### **LIVE SERVICES (1 Feature)**

1.  **FEATURE: AAA Live Ops Configuration Engine**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** A runtime-safe override system for dynamically tuning game parameters. It consists of a `LiveOpsManager` that fetches and validates configurations from a `RemoteConfigBridge`. It uses a `LiveOpsSafetyValidator` to clamp values and prevent exploits, ensuring that all modifications are temporary and non-destructive. The system respects all manager authorities by using a "pull" model, never directly mutating other systems.
    -   **GAPS:** None.

---

### **CORE GAMEPLAY (9 Features)**

2.  **FEATURE: Player Core Mechanics**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Handles player movement (lanes, jump, slide), collision, and health states.
    -   **GAPS:** Lacks advanced mechanics like wall-running and combo-based attacks.

3.  **FEATURE: Dynamic Level Generation**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Procedurally generates an endless experience using a tile-based system. Obstacle placement is now controlled by the `ProceduralPatternEngine`, which creates deterministic, fair, and skill-aware layouts. Legacy spawner scripts have been removed to fully centralize logic.
    -   **GAPS:** None.

4.  **FEATURE: Power-Up System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Manages the activation, duration, and fusion of in-game power-ups (Shield, Magnet, etc.). Now integrated with the Live Ops engine to allow for runtime duration modifications.
    -   **GAPS:** Fusion logic is implemented but lacks unique visual effects for each combination.

5.  **FEATURE: Score & Combo System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Tracks player score, combo multipliers, and style points.
    -   **GAPS:** None.

6.  **FEATURE: Momentum System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Rewards uninterrupted gameplay (`flow`) by granting escalating tiers of speed and score multipliers, resetting upon mistakes.
    -   **GAPS:** None.

7.  **FEATURE: Enemy & Obstacle System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Defines behaviors for various enemy types and dynamic obstacles. Obstacle placement is now fully managed by the `ProceduralPatternEngine`, ensuring patterns are validated and deterministic. All legacy spawning logic has been consolidated and removed.
    -   **GAPS:** The generation engine is complete, but the library of unique obstacle prefabs and enemy types with advanced AI remains to be expanded.

8.  **FEATURE: Boss Battle System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages unique, multi-phase boss encounters with special mechanics.
    -   **GAPS:** Entirely conceptual.

9.  **FEATURE: Revive & Checkpoint System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Allows the player to revive mid-run using currency or ads.
    -   **GAPS:** UI is present, but the ad-based revive logic is not linked to an Ad Manager.

10.  **FEATURE: Risk-Reward Lane System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** A dynamic system that periodically designates a single lane as high-risk, high-reward by increasing both obstacle and coin density. The system is event-driven and its parameters are controlled by remote config.
    -   **GAPS:** None.

---

### **VISUALS & THEMES (1 Feature)**

11. **FEATURE: AAA Rotating World Theme Engine**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Manages the automated weekly rotation and event-based override of the game's visual and audio themes. The system is data-driven, using `ThemeProfileData` ScriptableObjects to define every aspect of a theme, from skybox and lighting to materials and music. It is optimized for performance by using a `ThemeMaterialRegistry` to prevent runtime material duplication and ensures theme swaps only occur at the start of a run to avoid mid-game hitches.
    -   **GAPS:** None.

---

### **METAGAME & ECONOMY (9 Features)**

12. **FEATURE: Currency Management**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Manages all player currencies (Coins, Gems, Event Tokens). The `SaveManager` now persists currency data between sessions.
    -   **GAPS:** No central `CurrencyManager` script exists to unify transactions *during* gameplay.

13. **FEATURE: In-App Purchase (IAP) System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Handles real-money transactions for purchasing currencies and items via platform stores.
    -   **GAPS:** IAP SDK is not integrated.

14. **FEATURE: Ad Monetization System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages rewarded video ads, interstitials, and banners.
    -   **GAPS:** Ad SDK is not integrated.

15. **FEATURE: In-Game Shop**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** UI and logic for players to spend currency on items, skins, and boosts.
    -   **GAPS:** Shop UI and product database are not created.

16. **FEATURE: Mission & Quest System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Manages daily, weekly, and storyline quests with rewards. The `SaveManager` now persists the state of active quests.
    -   **GAPS:** A `MissionManager` exists, but lacks a connection to a UI to display quest progress to the player.

17. **FEATURE: Character & Skin System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Allows players to unlock and equip different character skins with potential passive bonuses.
    -   **GAPS:** No skin selection UI or data structure exists.

18. **FEATURE: Skill Tree & Progression**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Permanent player upgrades purchased with a specific currency.
    -   **GAPS:** Entirely conceptual.

19. **FEATURE: Daily Rewards & Login Bonus**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages rewards for consecutive daily logins.
    -   **GAPS:** Entirely conceptual.

20. **FEATURE: AAA Rare Drop & Legendary Shard Engine**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** An economy-safe, anti-exploit system for awarding rare items and legendary shards at the end of a run. It consists of the `RareDropManager` for calculating drops, `PityCounterManager` for bad luck protection, `ShardInventoryManager` for tracking and converting item fragments, and a `DropIntegrityValidator` to prevent cheating.
    -   **GAPS:** None.

---

### **TECHNICAL & UI (10 Features)**

21. **FEATURE: UI Management System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Controls all UI panels, popups, and HUD elements.
    -   **GAPS:** Score and Fusion UI are working, but currency, active power-ups, and mission status are not displayed.

22. **FEATURE: Game State Management**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Manages core game states (`Menu`, `Playing`, `Paused`, `EndOfRun`). A foundational, stable system that utilizes a refined Singleton pattern for manager access.
    -   **GAPS:** None.

23. **FEATURE: Data Persistence (Save/Load)**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** A unified `SaveManager` handles serialization of all critical game data, including player position, health, inventory, and quest status. It includes a checksum verification system to prevent data tampering.
    -   **GAPS:** None.

24. **FEATURE: Audio Management**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Controls background music, sound effects, and UI sounds with volume controls. The `WorldThemeManager` now handles the selection and playback of background music based on the active theme profile.
    -   **GAPS:** A central `AudioManager` for handling SFX and UI sounds is still needed.

25. **FEATURE: Input Management**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Uses Unity's Input System to handle swipe and touch controls. Foundational and stable.
    -   **GAPS:** None.

26. **FEATURE: Remote Config Integration**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Remote configuration of game parameters is managed through the `LiveOpsManager`. This allows for dynamic tuning of the game balance and features without requiring a new client build. The system is fully integrated.
    -   **GAPS:** None.

27. **FEATURE: Notification System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages local and push notifications to re-engage players.
    -   **GAPS:** Entirely conceptual.

28. **FEATURE: Error Handling & Logging**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Critical systems now include robust dependency checks and error logging to prevent fatal crashes during initialization.
    -   **GAPS:** Lacks a centralized, cloud-based error logging service.

29. **FEATURE: Tutorial System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Guides new players through core mechanics in a scripted sequence.
    -   **GAPS:** Entirely conceptual.

30. **FEATURE: AAA Player Analytics Engine**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** A lightweight, foundational analytics system designed to track player behavior without impacting performance. It consists of the `PlayerAnalyticsManager` (the central singleton), `SessionAnalyticsData` (a per-run data container with detailed, timestamped events), `BehaviorTrendAnalyzer` (for long-term pattern analysis like rage-quitting), and a `FrustrationDetector` (which uses rules like "quick successive deaths" to provide hooks for adaptive systems). The architecture is decoupled and provides a robust framework for future analytics expansion.
    -   **GAPS:** None.
