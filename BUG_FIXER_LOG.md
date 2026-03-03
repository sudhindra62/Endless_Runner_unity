# BUG_FIXER_LOG.md: The QA Lead & Automation Record
**ROLE:** A permanent, append-only log of all identified bugs, their root cause, and the exact steps taken to resolve them. This file serves as the primary record for the "One-Click Fix" protocol.
**AUTO-UPDATING LOGIC:** When the AI Guardian encounters a new runtime error or compilation failure, it will automatically create an entry here before attempting a fix. Once the fix is verified, it will update the entry with the solution.

---

### "One-Click Fix" Protocol

The "One-Click Fix" protocol is a directive for the AI Guardian to autonomously identify, analyze, and resolve critical issues within the project. The process is as follows:

1.  **Error Detection:** The AI Guardian continuously monitors the project for runtime exceptions, compilation errors, and logical inconsistencies identified through system audits.
2.  **Log Creation:** Upon detecting an error, the AI Guardian immediately creates a new entry in this log file, marking the status as `[UNRESOLVED]`. The entry includes the error message, stack trace, and the suspected location.
3.  **Root Cause Analysis:** The AI Guardian performs a deep analysis of the surrounding code, project files, and the `INTEGRATION_MAP.md` to determine the root cause of the issue.
4.  **Solution Formulation:** The AI Guardian formulates a corrective action, which may involve rewriting code, modifying project files, or updating configuration settings.
5.  **Automated Fix & Verification:** The AI Guardian applies the fix and then runs a series of verification checks to ensure the issue is resolved and no new issues have been introduced.
6.  **Log Update:** Once the fix is verified, the AI Guardian updates the corresponding log entry with the solution details and marks the status as `[VERIFIED_FIXED]`.

---

### [RESOLVED]
- **BUG ID:** 20240524-001
- **STATUS:** `[VERIFIED_FIXED]`
- **ERROR MESSAGE:** `NullReferenceException: Object reference not set to an instance of an object.`
- **LOCATION:** `ReviveManager.cs`
- **REPLICATION STEPS:**
  1. Player dies during a run.
  2. The Revive UI appears.
  3. Player clicks the "Revive" button.
- **ROOT CAUSE ANALYSIS:** The `ReviveManager` script was attempting to call a method on the `UIManager` instance, but the `UIManager` instance was not yet available in the scene, leading to a `NullReferenceException`.
- **SOLUTION:** Modified `ReviveManager.cs` to use the Singleton pattern (`UIManager.Instance`) to ensure it always has a valid reference to the `UIManager`.

### [RESOLVED]
- **BUG ID:** 20240524-002
- **STATUS:** `[VERIFIED_FIXED]`
- **ERROR MESSAGE:** `In-app purchase transaction remains pending and does not complete.`
- **LOCATION:** `ShopManager.cs`
- **REPLICATION STEPS:**
  1. Player initiates an in-app purchase.
  2. The purchase is successful on the platform side.
  3. The player is charged, but the item is not granted in the game.
- **ROOT CAUSE ANALYSIS:** The `ShopManager.cs` script was processing the purchase and calling the `EntitlementResolver`, but it was not confirming the purchase with the `IAPManager`. This left the transaction in a pending state.
- **SOLUTION:** Modified `ShopManager.cs` to call `IAPManager.Instance.ConfirmPurchase(productId)` after the `EntitlementResolver` successfully resolves the purchase, ensuring the transaction is completed.

### [RESOLVED]
- **BUG ID:** 20240523-001
- **STATUS:** `[VERIFIED_FIXED]`
- **ERROR MESSAGE:** `ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.`
- **LOCATION:** `StyleManager.cs: line 38`
- **REPLICATION STEPS:**
  1. A new theme is unlocked through an in-app purchase.
  2. Immediately starting a new run could cause a crash.
- **ROOT CAUSE ANALYSIS:** The `StyleManager` was not correctly updating its internal list of available themes after a purchase, causing it to try and select an index that was temporarily out of bounds.
- **SOLUTION:** Forced the `StyleManager` to re-initialize its theme list from the `PlayerData` object immediately after a successful theme purchase event was fired.
