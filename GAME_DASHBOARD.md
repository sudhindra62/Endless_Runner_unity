# GAME_DASHBOARD.md: The Owner’s Steering Wheel
**ROLE:** A human-readable dashboard for controlling all critical gameplay variables. This is the single source of truth for game balance.
**AUTO-UPDATING LOGIC:** When a value is changed in this file, the AI Guardian is responsible for finding the corresponding C# script and updating the variable to match. This ensures the Owner has direct control over the game feel without touching code.

---

### Player Movement
- **Forward Speed (Initial):** `10.0`
- **Lane Change Speed:** `15.0`
- **Jump Force:** `12.0`
- **Gravity Multiplier:** `3.0`
- **Slide Duration:** `0.7`
- **Stumble Duration (On Hit):** `1.0`

### Scoring & Multipliers
- **Base Score Per Second:** `5`
- **Coin Value (Score):** `25`
- **Near-Miss Score Bonus:** `100`
- **Flow Combo Multiplier Increment:** `0.05`

### Economy & Currency
- **Coins Per Run (Average Target):** `150`
- **Gems Per Run (Rare Chance %):** `0.5`
- **Cost to Revive:** `1` (Gem)

### Power-Up Balance
- **Magnet Duration:** `8.0`
- **Score Multiplier Duration:** `8.0`
- **Shield Duration:** `8.0`
- **Fever Mode Threshold:** `1500` (Fever Points)
- **Fever Mode Duration:** `10.0`

### Adaptive Difficulty
- **Max Difficulty Modifier:** `2.0`
- **Min Difficulty Modifier:** `0.5`
- **Difficulty Adjustment Rate:** `0.1`
- **Obstacle Density Increase (On Success):** `15%` (1.15x)
- **Obstacle Density Decrease (On Fail):** `-15%` (0.85x)
- **Coin Density Increase (On Fail):** `15%` (1.15x)

---

## Project Health & Next Steps

**AI GUARDIAN STATUS:** `Awaiting developer command.`
**PROJECT HEALTH:** [STABLE]

### **RECENT ACTIVITY**
- **[USER]** Requested a new health file to be added to the project.
- **[AI]** Added a health section to the GAME_DASHBOARD.md file.
- **[USER]** Reverted some of the recent changes.
- **[AI]** Reverted the changes as requested.

### **UPCOMING TASKS**
-   **[HIGH]** **Boss Chase Sequence:** The core logic for the boss chase is in place, but requires final polish.
    -   VFX for the boss's attacks are missing.
    -   SFX for the boss's movements and attacks are placeholders.
-   **[MEDIUM]** **Power-Up VFX:** The visual effects for the Magnet and Shield power-ups are basic and could be improved.
-   **[LOW]** **Store UI:** The in-game store for purchasing cosmetic items needs a UI refresh.

### **DEPLOYMENT READINESS**
-   **[OK]** **Android Build:** The project builds successfully for Android.
-   **[NEEDS ATTENTION]** **iOS Build:** The project has not been tested on an iOS device.
-   **[ACTION NEEDED]** **Publishing Checklist:**
    -   **[CRITICAL]** **App Store Screenshots:** New screenshots need to be generated for the App Store and Google Play listings.
    -   **[CRITICAL]** **Privacy Policy:** The game's privacy policy needs to be finalized and linked within the app.
    -   **[ACTION NEEDED]** **Google Play Console's Data Safety form is incomplete.
    -   **[CRITICAL]** **Content Rating:** The official content rating questionnaire must be completed.
    -   **[ACTION NEEDED]** **Monetization Declaration:** You must declare if the app uses Ads or IAP.
