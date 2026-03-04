# ERROR_HANDLING_POLICY.md: The Safety Officer
**ROLE:** Establishes the definitive rules for building a crash-resistant, production-ready application.
**AUTO-UPDATING LOGIC:** The AI Guardian must adhere to these policies in all code it generates. This file is its "Prime Directive" for stability. Any deviation requires explicit Owner override.

---

### 1. The Null-Check Protocol
- **Rule:** Before accessing any object that *could* be null, a check must be performed. This is non-negotiable for `GetComponent`, object lookups, or objects that might be destroyed.
- **Good Example:**
  ```csharp
  var uiManager = ServiceLocator.Instance.Get<UIManager>();
  if (uiManager != null)
  {
      uiManager.UpdateScore(newScore);
  }
  ```
- **Bad Example:**
  ```csharp
  ServiceLocator.Instance.Get<UIManager>().UpdateScore(newScore); // This is forbidden.
  ```

### 2. The Anti-Freeze Protocol
- **Rule:** No single operation on the main thread should ever block for more than a few frames. Long-running processes must be offloaded to Coroutines or background threads.
- **Policy:** The game must always feel responsive. A frozen UI or stuttering gameplay due to a blocking operation is considered a critical bug.
- **Example (File I/O):**
  ```csharp
  // BAD: Synchronous load that can freeze the game
  public void LoadGameData() {
      string data = File.ReadAllText(path);
      // ...process data
  }

  // GOOD: Asynchronous load using a Coroutine
  public IEnumerator LoadGameDataAsync() {
      UnityWebRequest request = UnityWebRequest.Get(path);
      yield return request.SendWebRequest();
      string data = request.downloadHandler.text;
      // ...process data
  }
  ```

### 3. Graceful Failure for Web Services
- **Rule:** All interactions with external services (Ads, IAP, Remote Config, Leaderboards) MUST be wrapped in `try-catch` blocks or use asynchronous callbacks with explicit failure handlers.
- **Policy:** The game must function perfectly offline. A failed ad load or inability to connect to Firebase cannot, under any circumstances, cause a crash or prevent the player from playing.
- **Example (Ads):**
  ```csharp
  public void ShowRewardedAd()
  {
      if (Advertisement.IsReady("rewardedVideo"))
            {
          Advertisement.Show("rewardedVideo");
      }
      else
      {
          Debug.LogWarning("Rewarded ad was not ready. Failing gracefully.");
          // Optionally, show a message to the user: "Sorry, no ads available right now."
      }
  }
  ```

### 4. Defensive `GetComponent` and `FindObjectOfType`
- **Rule:** Use `[RequireComponent(typeof(Rigidbody))]` in classes that depend on another component to enforce Inspector-level dependencies.
- **Policy:** Avoid using `FindObjectOfType` in `Update()` or other frequently-called methods. If an object must be found, cache the reference in `Awake()` or `Start()` and perform a single null check.
- **Hierarchy:** Prefer `ServiceLocator` or direct Inspector references over searching the scene.

### 5. Assume Invalid Player Input
- **Rule:** Do not assume player input will be correct. Clamp values, handle edge cases, and sanitize any text input.
- **Example:** When spending currency, always check if the player has enough *before* subtracting the cost.
  ```csharp
  public bool PurchaseItem(int cost)
  {
      if (playerCoins >= cost)
      {
          playerCoins -= cost;
          return true; // Success
      }
      return false; // Failure
  }
  ```

### 6. Log, Don't Crash
- **Rule:** When a non-fatal error occurs, log it to the console with `Debug.LogWarning` or `Debug.LogError` but allow the game to continue. Crashing is the last resort.
- **Policy:** A broken UI element should disappear or show default text; it should not halt the entire game.
