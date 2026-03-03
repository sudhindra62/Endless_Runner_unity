# FEATURE_CHECKLIST.md: The Project Manager

**ROLE:** Provides a granular, task-oriented view of each feature's development lifecycle.
**AUTO-UPDATING LOGIC:** The AI Guardian references this file to understand the sub-tasks required for implementing new features and will update the status as work is completed.

---

### **CORE GAMEPLAY**

-   **FEATURE: Player Core Mechanics** - `[PARTIAL]`
    -   `[DONE]` Basic Movement (Lanes, Jump, Slide)
    -   `[DONE]` Collision Detection & Death
    -   `[PLANNED]` Wall-Running Mechanic
    -   `[PLANNED]` Combo-Based Attack Moves

-   **FEATURE: Dynamic Level Generation** - `[PARTIAL]`
    -   `[DONE]` Procedural Tile Spawning
    -   `[PLANNED]` Dynamic Obstacle Difficulty Scaling
    -   `[PLANNED]` Themed Zone Transitions

-   **FEATURE: Power-Up System** - `[PARTIAL]`
    -   `[DONE]` PowerUpManager & ScriptableObject Base
    -   `[DONE]` PowerUpFusionManager Logic
    -   `[PLANNED]` Unique VFX for each Fusion

-   **FEATURE: Score & Combo System** - `[PARTIAL]`
    -   `[DONE]` Score accumulation and `OnScoreChanged` event
    -   `[PLANN-ED]` Style Point Triggers (e.g., Near-Miss, Perfect Dodge)

-   **FEATURE: Enemy & Obstacle System** - `[PLANNED]`
    -   `[PLANNED]` AI Behavior Tree for Enemies
    -   `[PLANNED]` Obstacle variants with dynamic properties

-   **FEATURE: Boss Battle System** - `[PLANNED]`
    -   `[PLANNED]` Multi-phase boss fight scripting
    -   `[PLANNED]` Unique boss intro and outro cutscenes

-   **FEATURE: Revive & Checkpoint System** - `[PARTIAL]`
    -   `[DONE]` Revive UI Panel
    -   `[PLANNED]` Ad-based Revive Logic Integration
    -   `[PLANNED]` Mid-run Checkpoint Save States

---

### **METAGAME & ECONOMY**

-   **FEATURE: Currency Management** - `[PLANNED]`
    -   `[PLANNED]` Central `CurrencyManager` script
    -   `[PLANNED]` UI bindings for Coin and Gem displays

-   **FEATURE: In-App Purchase (IAP) System** - `[PLANNED]`
    -   `[PLANNED]` IAP SDK Initialization
    -   `[PLANNED]` Consumable and Non-Consumable Product Definitions

-   **FEATURE: Ad Monetization System** - `[PLANNED]`
    -   `[PLANNED]` Ad SDK Initialization
    -   `[PLANNED]` Rewarded Ad Placements (e.g., for Revive, End-of-Run Bonus)

-   **FEATURE: In-Game Shop** - `[PLANNED]`
    -   `[PLANNED]` Shop UI with tabbed categories
    -   `[PLANNED]` Product ScriptableObject Database

-   **FEATURE: Mission & Quest System** - `[PLANNED]`
    -   `[PLANNED]` `MissionManager` with daily/weekly quest logic
    -   `[PLANNED]` Quest progress tracking and reward disbursement

-   **FEATURE: Character & Skin System** - `[PLANNED]`
    -   `[PLANNED]` Skin selection UI
    -   `[PLANNED]` `SkinData` ScriptableObjects with passive bonuses

-   **FEATURE: Skill Tree & Progression** - `[PLANNED]`
    -   `[PLANNED]` Skill Tree UI with unlockable nodes
    -   `[PLANNED]` `SkillNodeData` ScriptableObjects for permanent upgrades

-   **FEATURE: Daily Rewards & Login Bonus** - `[PLANNED]`
    -   `[PLANNED]` UI for daily reward calendar
    -   `[PLANNED]` Logic to track consecutive login days

---

### **TECHNICAL & UI**

-   **FEATURE: UI Management System** - `[PARTIAL]`
    -   `[DONE]` Core Panel Switching (Menu, Gameplay, Pause)
    -   `[DONE]` Score and Fusion UI display
    -   `[PLANNED]` HUD elements for currency and active power-ups

-   **FEATURE: Game State Management** - `[DONE]`
    -   `[DONE]` State machine with `OnGameStateChanged` event
    -   `[DONE]` Time scale management

-   **FEATURE: Data Persistence (Save/Load)** - `[PLANNED]`
    -   `[PLANNED]` `SaveManager` using `BinaryFormatter` or `JsonUtility`
    -   `[PLANNED]` Data encryption for security

-   **FEATURE: Audio Management** - `[PLANNED]`
    -   `[PLANNED]` Central `AudioManager` with sound mixing groups
    -   `[PLANNED]` UI for volume controls

-   **FEATURE: Input Management** - `[DONE]`
    -   `[DONE]` Swipe and touch controls via Unity's Input System

-   **FEATURE: Analytics & Remote Config** - `[PLANNED]`
    -   `[PLANNED]` Firebase SDK integration
    -   `[PLANNED]` `RemoteConfigBridge` to fetch values from `GAME_DASHBOARD.md`

-   **FEATURE: Notification System** - `[PLANNED]`
    -   `[PLANNED]` Local notifications for re-engagement
    -   `[PLANNED]` Push notification setup (requires server)

-   **FEATURE: Error Handling & Logging** - `[PLANNED]`
    -   `[PLANNED]` Centralized error logging service (e.g., Sentry, Firebase Crashlytics)

-   **FEATURE: Tutorial System** - `[PLANNED]`
    -   `[PLANNED]` Scripted, event-driven tutorial sequence
    -   `[PLANNED]` UI overlays to guide the player
