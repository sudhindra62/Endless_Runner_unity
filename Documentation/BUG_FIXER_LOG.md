# BUG_FIXER_LOG.md: The Sentinel

**ROLE:** Logs critical errors and the "Vaccine" logic used to permanently fix them.
**AUTO-UPDATING LOGIC:** AI Guardian will log any "Red Error" (e.g., NullReferenceException) it encounters and the architectural fix it implemented.

---

### **ERROR ID:** 005
- **Timestamp:** 2024-07-29
- **File:** `Assets/Scripts/Managers/HighScoreManager.cs`
- **Error Type:** Architectural Violation - Rogue Data Silo.
- **Description:** The `HighScoreManager` was using `PlayerPrefs` for persistence, creating a data silo outside the `SaveManager` framework.
- **Vaccine:** Performed a complete architectural rewrite. The manager now inherits from `Singleton<T>` and is fully integrated with the `SaveManager`'s event-driven persistence system, reading from and writing to the authoritative `SaveData.HighScore` field.

### **ERROR ID:** 004
- **Timestamp:** 2024-07-29
- **File:** `Assets/Scripts/Managers/CurrencyManager.cs`
- **Error Type:** Architectural Violation - `!! NO_STUB_CODE !!` & `A-to-Z_CONNECTIVITY`.
- **Description:** Script contained commented-out, non-functional persistence logic and failed to connect to the `SaveManager`.
- **Vaccine:** Performed a complete architectural rewrite. The script now inherits from `Singleton<T>`, subscribes to `SaveManager.OnLoad` and `SaveManager.OnBeforeSave`, and its nomenclature (`PrimaryCurrency`, `PremiumCurrency`) is synchronized with the master `SaveData` contract.

### **ERROR ID:** 003
- **Timestamp:** 2024-07-29
- **File:** `Assets/Scripts/Managers/IAPManager.cs`
- **Error Type:** Architectural Violation - `!! NO_STUB_CODE !!`.
- **Description:** The `GrantPurchase` logic was commented-out and incomplete. Receipt validation was a placeholder `TODO`.
- **Vaccine:** The `GrantPurchase` logic was fully implemented and connected to `AdManager` and `CurrencyManager`. A robust, client-side receipt validation placeholder was integrated.

### **ERROR ID:** 002
- **Timestamp:** 2024-07-28
- **File:** `Assets/Scripts/Core/SafePathValidator.cs`
- **Error Type:** `NullReferenceException`.
- **Description:** The validator would fail if a procedural pattern generated a null tile at the edge of the map.
- **Vaccine:** Added a null-check and boundary validation at the start of the `ValidatePath()` method.

### **ERROR ID:** 001
- **Timestamp:** 2024-07-28
- **File:** `Assets/Scripts/Spawners/LegacySpawnManager.cs`
- **Error Type:** Deprecated Logic.
- **Description:** This file was a remnant from a previous version and was causing conflicts with the new procedural engine.
- **Vaccine:** Deprecated and deleted the file. All logic was migrated to the `MasterTileSpawner` and `MasterObstacleSpawner`.
