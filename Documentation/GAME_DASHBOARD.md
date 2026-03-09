# GAME_DASHBOARD.md: The Bridge

**ROLE:** A live dashboard showing the game's core metrics and design values.
**AUTO-UPDATING LOGIC:** AI Guardian will update these values whenever a design change is made in a relevant manager or data script.

---

### Core Gameplay Loop
- **Player Speed (Base):** `15.0f`
- **Score Multiplier (Default):** `1.0x`
- **Run Start Delay:** `1.0s`

### Economy & Monetization
- **Primary Currency Name:** `Coins`
- **Premium Currency Name:** `Gems`
- **Initial Primary Currency:** `0`
- **Initial Premium Currency:** `0`
- **Score-to-Coin Conversion:** `100 score = 1 Coin`

### IAP Products (Google Play / Apple App Store)
- **Remove Ads:** `com.gamestudio.remove_ads` (Non-Consumable)
- **Gem Pack 1 (100 Gems):** `com.gamestudio.gem_pack_1`
- **Gem Pack 2 (550 Gems):** `com.gamestudio.gem_pack_2`
- **Gem Pack 3 (1200 Gems):** `com.gamestudio.gem_pack_3`
- **Starter Bundle (200 Gems + Items):** `com.gamestudio.starter_bundle`
