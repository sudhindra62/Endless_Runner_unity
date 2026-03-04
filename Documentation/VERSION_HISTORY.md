# VERSION_HISTORY.md: The Historian

**ROLE:** A chronological log of all major project milestones and build versions.
**AUTO-UPDATING LOGIC:** After a significant feature is implemented or a new build is deployed, the AI Guardian will append an entry to this file, summarizing the changes.

---

### **v0.2.2 (Project Cleanup & Structural Integrity)**

-   **DATE:** `2024-05-25`
-   **MILESTONE:** Project Cleanup & Structural Integrity.
-   **CHANGES:**
    -   **File Consolidation:** Merged `SaveSystem.cs` into `SaveManager.cs` to create a single, unified data persistence system.
    -   **Duplicate Removal:** Deleted the redundant `Singleton.cs` script and moved all documentation files to the `Documentation` folder.
    -   **Code Cleanup:** Removed the unnecessary `GameManager.cs` and the deprecated `ObstacleSpawner.cs`.
-   **STATUS:** The project is now cleaner, more organized, and easier to maintain.

### **v0.2.1 (Architectural Consolidation)**

-   **DATE:** `2024-05-25`
-   **MILESTONE:** Documentation Consolidation.
-   **CHANGES:**
    -   **Merged `project_architecture.md`:** The redundant `project_architecture.md` file was merged into `VERSION_HISTORY.md` and `INTEGRATION_MAP.md`.
    -   **Refactoring Log:**
        -   **Obstacle Spawning:** Refactored the `ObstacleSpawner` to be a simple executor. The logic for what to spawn has been moved to the `ProceduralPatternEngine`.
        -   **Coin Spawning:** The `CoinSpawner` now listens to the `EffectsManager` to handle the obstacle-to-coin conversion effect.

### **v0.2.0 (Procedural Engine Integration)**

-   **DATE:** `2024-05-25`
-   **MILESTONE:** AAA Procedural Obstacle Pattern Engine integration.
-   **CHANGES:**
    -   **Created Core Engine:** Implemented `ProceduralPatternEngine.cs`, `PatternRuleValidator.cs`, `PatternSeedManager.cs`, `PatternDifficultyProfile.cs`, and `SafePathValidator.cs`.
    -   **Deterministic Generation:** Runs are now 100% deterministic and replayable via a master seed from `RunSessionData`.
    -   **Fairness Validation:** The `SafePathValidator` now guarantees every pattern has a survivable path, preventing impossible layouts.
    -   **Skill-Aware Adaptation:** Engine subscribes to `EffectsManager.OnNearMiss` to dynamically adjust obstacle density based on player skill.
    -   **Spawner Refactor:** `MasterObstacleSpawner` is now a pure executor, listening for instructions from the engine, as per the "Zero Authority Conflict" mandate.
-   **STATUS:** All primary technical requirements implemented. Ready for dependency mapping and commercial-grade feature integration.

### **v0.1.0 (Foundation & Initial Setup)**

-   **DATE:** `2024-05-24`
-   **MILESTONE:** System Integrity Audit & 11-File Master Infrastructure Activation.
-   **CHANGES:**
    -   Audited and repaired all 11 core architectural files for compliance.
    -   Established the "Owner-Controlled Ecosystem" protocol.
    -   Initial project setup with core systems and placeholder player art.
