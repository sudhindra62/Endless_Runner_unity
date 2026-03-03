# MASTER_FEATURE_REGISTRY.md: The Project Dictionary

**PROTOCOL:** This file is the definitive, auto-updating dictionary of every feature in the project. It is maintained by the AI Guardian and serves as the primary source of truth for system architecture and integration status.

---

### **CORE GAMEPLAY (9 Features)**

1.  **FEATURE: Player Core Mechanics**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Handles player movement (lanes, jump, slide), collision, and health states.
    -   **GAPS:** Lacks advanced mechanics like wall-running and combo-based attacks.

2.  **FEATURE: Dynamic Level Generation**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Procedurally generates an endless experience using a tile-based system. Obstacle placement is now controlled by the `ProceduralPatternEngine`, which creates deterministic, fair, and skill-aware layouts.
    -   **GAPS:** None.

3.  **FEATURE: Power-Up System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Manages the activation, duration, and fusion of in-game power-ups (Shield, Magnet, etc.).
    -   **GAPS:** Fusion logic is implemented but lacks unique visual effects for each combination.

4.  **FEATURE: Score & Combo System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Tracks player score, combo multipliers, and style points.
    -   **GAPS:** None.

5.  **FEATURE: Momentum System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Rewards uninterrupted gameplay (`flow`) by granting escalating tiers of speed and score multipliers, resetting upon mistakes.
    -   **GAPS:** None.

6.  **FEATURE: Enemy & Obstacle System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Defines behaviors for various enemy types and dynamic obstacles. Obstacle placement is now fully managed by the `ProceduralPatternEngine`, ensuring patterns are validated and deterministic.
    -   **GAPS:** The generation engine is complete, but the library of unique obstacle prefabs and enemy types with advanced AI remains to be expanded.

7.  **FEATURE: Boss Battle System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages unique, multi-phase boss encounters with special mechanics.
    -   **GAPS:** Entirely conceptual.

8.  **FEATURE: Revive & Checkpoint System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Allows the player to revive mid-run using currency or ads.
    -   **GAPS:** UI is present, but the ad-based revive logic is not linked to an Ad Manager.

9.  **FEATURE: Risk-Reward Lane System**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** A dynamic system that periodically designates a single lane as high-risk, high-reward by increasing both obstacle and coin density. The system is event-driven and its parameters are controlled by remote config.
    -   **GAPS:** None.

---

### **METAGAME & ECONOMY (8 Features)**

10.  **FEATURE: Currency Management**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages all player currencies (Coins, Gems, Event Tokens).
    -   **GAPS:** No central `CurrencyManager` script exists to unify transactions.

11. **FEATURE: In-App Purchase (IAP) System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Handles real-money transactions for purchasing currencies and items via platform stores.
    -   **GAPS:** IAP SDK is not integrated.

12. **FEATURE: Ad Monetization System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages rewarded video ads, interstitials, and banners.
    -   **GAPS:** Ad SDK is not integrated.

13. **FEATURE: In-Game Shop**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** UI and logic for players to spend currency on items, skins, and boosts.
    -   **GAPS:** Shop UI and product database are not created.

14. **FEATURE: Mission & Quest System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages daily, weekly, and storyline quests with rewards.
    -   **GAPS:** `MissionManager` and quest database are conceptual.

15. **FEATURE: Character & Skin System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Allows players to unlock and equip different character skins with potential passive bonuses.
    -   **GAPS:** No skin selection UI or data structure exists.

16. **FEATURE: Skill Tree & Progression**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Permanent player upgrades purchased with a specific currency.
    -   **GAPS:** Entirely conceptual.

17. **FEATURE: Daily Rewards & Login Bonus**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages rewards for consecutive daily logins.
    -   **GAPS:** Entirely conceptual.

---

### **TECHNICAL & UI (9 Features)**

18. **FEATURE: UI Management System**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Controls all UI panels, popups, and HUD elements.
    -   **GAPS:** Score and Fusion UI are working, but currency, active power-ups, and mission status are not displayed.

19. **FEATURE: Game State Management**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Manages core game states (`Menu`, `Playing`, `Paused`, `EndOfRun`). A foundational, stable system.
    -   **GAPS:** None.

20. **FEATURE: Data Persistence (Save/Load)**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Saves and loads player progress, currency, and settings locally.
    -   **GAPS:** No `SaveManager` is implemented.

21. **FEATURE: Audio Management**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Controls background music, sound effects, and UI sounds with volume controls.
    -   **GAPS:** No central `AudioManager` exists.

22. **FEATURE: Input Management**
    -   **STATUS:** [COMPLETE]
    -   **DESCRIPTION:** Uses Unity's Input System to handle swipe and touch controls. Foundational and stable.
    -   **GAPS:** None.

23. **FEATURE: Analytics & Remote Config**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** The `ProceduralPatternEngine` now includes the logic for skill-aware difficulty adaptation based on player performance metrics.
    -   **GAPS:** Firebase SDK for sending analytics events and fetching remote configurations is not yet integrated.

24. **FEATURE: Notification System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Manages local and push notifications to re-engage players.
    -   **GAPS:** Entirely conceptual.

25. **FEATURE: Error Handling & Logging**
    -   **STATUS:** [PARTIAL]
    -   **DESCRIPTION:** Critical systems now include robust dependency checks and error logging to prevent fatal crashes during initialization.
    -   **GAPS:** Lacks a centralized, cloud-based error logging service.

26. **FEATURE: Tutorial System**
    -   **STATUS:** [PLANNED]
    -   **DESCRIPTION:** Guides new players through core mechanics in a scripted sequence.
    -   **GAPS:** Entirely conceptual.