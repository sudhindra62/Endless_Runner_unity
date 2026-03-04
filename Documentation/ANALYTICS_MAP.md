# ANALYTICS_MAP.md: The Data Scientist
**ROLE:** Defines the taxonomy of all custom analytics events to be tracked in the game. This is crucial for understanding player behavior and optimizing the experience.
**AUTO-UPDATING LOGIC:** When implementing a feature that has a business-critical outcome (e.g., using a power-up, making a purchase), the AI Guardian will consult this map and fire the appropriate Firebase Analytics event with the required parameters.

---

### Visuals & Theming
- **`theme_applied`**
  - *Trigger:* When a theme is applied at the start of a run.
  - *Parameters:* `{ "theme_id": "Jungle_01", "source": "weekly_rotation" | "event_override" | "milestone_unlock" | "remote_config" }`
- **`theme_previewed`**
  - *Trigger:* When a player previews a theme in the UI (placeholder for future UI features).
  - *Parameters:* `{ "theme_id": "Jungle_01" }`

### Core Gameplay Loop
- **`run_started`**
  - *Trigger:* When a player begins a new run.
  - *Parameters:* `{ "selected_char": "char_name", "active_modifier": "modifier_name" }`
- **`run_ended`**
  - *Trigger:* When a player's run is over.
  - *Parameters:* `{ "score": 12345, "coins_collected": 123, "distance_m": 456.7, "duration_sec": 90.5 }`
- **`player_death`**
  - *Trigger:* At the moment of player death/crash.
  - *Parameters:* `{ "obstacle_type": "HighObstacle_A", "distance_m": 450.1, "is_revive_offered": true }`
- **`coin_collect`**
    - *Trigger:* When a player collides with a coin collectible.
    - *Parameters:* `{ "coin_type": "standard", "value": 1 }`

### Monetization & Economy
- **`iap_purchase_attempted`**
  - *Trigger:* When a player clicks on an IAP item in the shop.
  - *Parameters:* `{ "product_id": "com.mygame.coin_pile" }`
- **`iap_purchase_completed`**
  - *Trigger:* After a successful IAP transaction.
  - *Parameters:* `{ "product_id": "com.mygame.coin_pile", "price_usd": 0.99 }`
- **`rewarded_ad_offered`**
  - *Trigger:* When a rewarded ad prompt is shown (e.g., for a revive).
  - *Parameters:* `{ "ad_placement": "revive_prompt" }`
- **`rewarded_ad_completed`**
  - *Trigger:* When the player successfully finishes watching a rewarded ad.
  - *Parameters:* `{ "ad_placement": "revive_prompt" }`
- **`shop_item_purchased`**
  - *Trigger:* When a player buys an item with in-game currency.
  - *Parameters:* `{ "item_id": "upgrade_magnet_lvl2", "cost": 1000, "currency": "coins" }`

### Feature Engagement
- **`powerup_collected`**
  - *Trigger:* When a player picks up any power-up.
  - *Parameters:* `{ "powerup_type": "Shield" }`
- **`powerup_used`**
    - *Trigger:* When a power-up's effect is activated.
    - *Parameters:* `{ "powerup_type": "Shield" }`
- **`mission_claimed`**
    - *Trigger:* When a player claims the reward for a completed mission.
    - *Parameters:* `{ "mission_id": "collect_500_coins", "reward_type": "coins", "reward_amount": 50 }`
- **`mission_completed`**
  - *Trigger:* When the player completes a mission.
  - *Parameters:* `{ "mission_id": "collect_500_coins" }`
- **`achievement_unlocked`**
  - *Trigger:* When a player unlocks an achievement.
  - *Parameters:* `{ "achievement_id": "first_run" }`
