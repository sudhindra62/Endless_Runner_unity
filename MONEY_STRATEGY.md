# MONEY_STRATEGY.md: The Business Director
**ROLE:** Defines the complete monetization and economy strategy for the game.
**AUTO-UPDATING LOGIC:** The AI Guardian uses this file as the rulebook for implementing IAP products and ad placements. Any change here will trigger a review of the `IAPManager`, `AdManager`, and Shop UI scripts.

---

### In-App Purchases (IAP)

**Currency Bundles:**
1.  **Coin Pile:** `1,000 Coins` - `$0.99`
2.  **Coin Satchel:** `5,500 Coins` - `$4.99` (10% Bonus)
3.  **Coin Chest:** `12,000 Coins` - `$9.99` (20% Bonus)
4.  **Gem Starter Pack:** `10 Gems` - `$1.99`
5.  **Gem Trove:** `50 Gems` - `$7.99`

**Special Offers:**
- **Starter Bundle:** `5,000 Coins + 10 Gems + 3 Headstarts` - `$2.99` (One-time purchase)
- **Disable Forced Ads:** `Remove interstitial ads forever` - `$4.99`

### Ad Monetization

**Rewarded Video Ads:**
- **Second Chance Revive:** Watch an ad to continue a run after crashing. (Limit: 1 per run).
- **Bonus Currency:** Watch an ad from the home screen to get `100 Coins`. (Limit: 5 per day).
- **Mission Skip:** Watch an ad to automatically complete a difficult mission.

**Interstitial Ads:**
- **Frequency:** Display one interstitial ad after every 3 runs.
- **Trigger:** Shown on the transition from the Run Summary screen back to the Home screen.
- **Exemption:** Do not show to users who have made any real-money purchase.

### In-Game Shop Prices

**Consumables (Bought with Coins):**
- **Headstart:** `500 Coins` (Start the run at high speed for 10 seconds)
- **Score Booster:** `750 Coins` (Start the run with a +5 score multiplier)

**Permanent Upgrades (Bought with Coins):**
- **Magnet Duration (Level 1-5):** `500, 1000, 2000, 4000, 8000`
- **Score Multiplier Duration (Level 1-5):** `500, 1000, 2000, 4000, 8000`
