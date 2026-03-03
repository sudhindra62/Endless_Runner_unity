
# MASTER FEATURE REGISTRY

This document is the "Source of Truth" for the Endless Runner project. It tracks all features, their implementation status, and identifies gaps for achieving a "Play Store Ready" product.

---

### Core Gameplay & Player Mechanics

**1. Flow Combo System**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `FlowComboManager.cs` tracks consecutive successful actions (like near-misses or perfect dodges). The combo count increases a score multiplier, managed via `OnComboChanged` and `OnComboMultiplierChanged` events. The combo breaks upon collision.
- **MISSING LINKS:** Requires robust UI feedback (e.g., visual flair, sound effects) to make the combo state clear and rewarding to the player.

**2. Near-Miss Bonus System**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `NearMissDetector.cs` attached to the player triggers an event when the player gets close to an obstacle without colliding. This event can be used to grant score bonuses and increment the Flow Combo.
- **MISSING LINKS:** Needs a dedicated scoring event and UI pop-up to explicitly reward the player for the near-miss.

**3. Character Passive Abilities**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `CharacterPassiveManager.cs` is responsible for applying passive buffs or abilities based on the currently selected character. The logic for these abilities is likely defined in `CharacterData.cs`.
- **MISSING LINKS:** The system is in place, but requires a wider variety of unique and balanced character passives to be designed and added.

**4. Momentum System**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** This system would cause the player's speed to gradually increase over the duration of a run, making the game progressively harder. Speed could also be influenced by collecting certain power-ups or hitting boost pads.
- **MISSING LINKS:** The `PlayerMovement.cs` script would need to be modified to include a variable for run speed that increases over time or based on events.

---

### Dynamic Run & World Systems

**5. Adaptive AI Difficulty**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `AdaptiveDifficultyManager.cs` monitors player performance (dodges, deaths, combo streaks) and adjusts game parameters in real-time. It can increase/decrease obstacle density and pattern complexity by invoking events like `OnDifficultyChanged`.
- **MISSING LINKS:** The system is functional, but could be expanded to control more variables, like enemy speed or power-up frequency.

**6. Boss Run Mode**
- **STATUS:** [PARTIAL]
- **LOGIC DESCRIPTION:** `BossChaseManager.cs` and `BossController.cs` exist, suggesting a mode where a boss chases the player. The core logic to trigger the chase and manage the boss's presence seems to be in place.
- **MISSING LINKS:** Requires specific boss attack patterns, unique UI for the boss health/distance, and defined start/end conditions for the boss run sequence.

**7. Random Run Modifiers**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `RunModifierManager.cs` selects and applies temporary changes to the game rules at the start of a run, such as "All Coins are Gems" or "Increased Power-Up Duration."
- **MISSING LINKS:** Needs a larger pool of interesting and varied modifiers to keep the gameplay fresh. UI elements are needed to inform the player which modifier is active.

**8. Dynamic Environment Events**
- **STATUS:** [PARTIAL]
- **LOGIC DESCRIPTION:** The `WorldEventManager.cs` is designed to trigger large-scale events during a run (e.g., a subway train passing, falling rocks). This system exists but seems to be a framework.
- **MISSING LINKS:** Requires the creation of specific, visually distinct events and the logic to spawn them and integrate them into the procedural generation.

**9. Rotating World Themes**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `ThemeManager.cs` allows for switching the visual appearance of the game world (e.g., city, jungle, volcano). It likely manages skyboxes, road textures, and obstacle prefabs associated with each theme.
- **MISSING LINKS:** Requires a greater variety of art assets (models, textures) to create more distinct and compelling themes.

**10. Procedural Obstacle Patterns**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `ObstacleSpawner.cs` and `TileSpawner.cs` work together to procedurally generate the game world. The system pulls from a pool of predefined obstacle formations to create an endless and varied track.
- **MISSING LINKS:** The core system is robust. Future work involves designing more complex and challenging obstacle patterns.

**11. Risk-Reward Lanes**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** This feature would involve lanes that are intentionally more dangerous but offer greater rewards (e.g., a lane filled with gems but also many obstacles).
- **MISSING LINKS:** `TileSpawner.cs` and `ObstacleSpawner.cs` would need to be updated to support the concept of "lane difficulty" and to procedurally generate these high-risk, high-reward paths.

**12. Decision Path System**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** At certain points in a run, the player would be presented with a choice of two or more paths, each with different themes, challenges, and rewards (e.g., "Left for the Cave, Right for the Bridge").
- **MISSING LINKS:** This is a significant architectural addition. It would require major updates to the `TileSpawner` to handle branching and merging paths.

---

### Power-Ups & Special Modes

**13. Power-Up Fusion**
- **STATUS:** [PARTIAL]
- **LOGIC DESCRIPTION:** The `PowerUpFusionManager.cs` and associated scripts in `Assets/Scripts/PowerUps/Fusions` provide a framework for combining two active power-ups into a new, more powerful version (e.g., Magnet + Coin Doubler = Coin Storm).
- **MISSING LINKS:** The UI for displaying potential fusions and the input mechanism for activating them are not implemented. More fusion combinations need to be designed.

**14. Fever Mode**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `FeverModeManager.cs` tracks a "Fever" meter that fills up as the player collects coins or performs special actions. When full, it triggers a high-score, low-risk mode where the player is invincible and surrounded by bonus items.
- **MISSING LINKS:** The visual and audio effects for Fever Mode could be enhanced to make it feel more impactful and exciting.

**15. Time Warp Mechanic**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** A rare power-up that temporarily slows down time, making it easier for the player to navigate complex obstacle patterns and collect items.
- **MISSING LINKS:** Requires a new power-up type and modifications to the game's `Time.timeScale`. This would need careful management to avoid conflicts with other systems.

---

### Meta-Progression & Retention

**16. Skill Tree System**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `SkillTreeManager.cs` and `SkillNodeUI.cs` provide a system for players to spend in-game currency on permanent upgrades, such as longer power-up durations or a higher starting score multiplier.
- **MISSING LINKS:** The system is functional. The key is designing a balanced and engaging set of skills for the player to unlock.

**17. League Ranked Mode**
- **STATUS:** [PARTIAL]
- **LOGIC DESCRIPTION:** The `LeagueManager.cs` exists to manage player progression through competitive tiers (e.g., Bronze, Silver, Gold) based on their high scores or weekly performance.
- **MISSING LINKS:** Requires a backend to support global leaderboards and a UI to show league rankings, promotion/demotion status, and end-of-season rewards.

**18. Spin-to-Win System**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** A retention mechanic where players can spin a wheel once per day to earn random rewards like currency, power-ups, or cosmetic items.
- **MISSING LINKS:** Requires a UI for the wheel, a backend prize table, and an inventory system to grant the rewards.

**19. Rare Drop Engine**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `RareDropManager.cs` handles the logic for awarding players with rare items (like cosmetic skins or currency bundles) based on a low probability chance during gameplay.
- **MISSING LINKS:** The system is functional. Needs a wider variety of rare drops to be created.

**20. “Almost Win” Retention Psychology**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** This involves designing systems that make the player feel like they were "so close" to a goal, encouraging another run. For example, if a player dies just before a new high score, the game might offer a free revive.
- **MISSING LINKS:** Requires modifications to the `ReviveManager` and `RunSummaryUI` to identify these "almost win" moments and present a compelling offer to the player.

**21. Weekly Global Events**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** Similar to `WorldEventManager`, but on a global scale. All players would work towards a collective goal over the week (e.g., "Collect 1 Billion Coins") for a global reward.
- **MISSING LINKS:** Requires a robust backend service to track global progress and distribute rewards.

---

### Multiplayer & Social

**22. Ghost Run System**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** This system would record a player's run (path, power-up usage, score) and allow other players to compete against a "ghost" of that performance in real-time.
- **MISSING LINKS:** Requires a system to record and serialize run data, and a "ghost" playback controller.

**23. Multiplayer Ghost Race**
- **STATUS:** [PLANNED_NOT_STARTED]
- **LOGIC DESCRIPTION:** An evolution of the Ghost Run, where players could race against the ghosts of several other players (or their friends) simultaneously.
- **MISSING LINKS:** This is a superset of the Ghost Run System and would require a more complex UI to track the positions of multiple ghosts.

---

### Discovered & Implemented Base Features

**24. Coin & Gem Currency System**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** `Coin.cs` and `CurrencyManager.cs` handle the collection and management of the game's primary and premium currencies.
- **MISSING LINKS:** Fully functional.

**25. Core Power-Ups (Magnet, Shield, Doubler)**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** Scripts like `Magnet.cs`, `Shield.cs`, and `CoinDoubler.cs` provide the classic endless runner power-up behaviors. Managed by the `PowerUpManager.cs`.
- **MISSING LINKS:** Fully functional.

**26. In-App Purchase (IAP) System**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `IAPManager.cs` provides the hooks for players to purchase currency or items with real money.
- **MISSING LINKS:** Requires setup with Google Play Console and a configured product catalog.

**27. Ad Monetization System**
- **STATUS:** [FULLY_IMPLEMENTED]
- **LOGIC DESCRIPTION:** The `AdManager.cs` handles the display of rewarded video ads, likely for extra currency or revives.
- **MISSING LINKS:** Requires an ad network SDK and placement configuration.

**28. Character Skin / Cosmetic System**
- **STATUS:** [PARTIAL]
- **LOGIC DESCRIPTION:** `SkinsManager.cs` and `SkinDataManager.cs` manage the unlocking and selection of different character appearances.
- **MISSING LINKS:** The backend is present, but it needs a robust UI for players to browse, purchase, and equip skins.

---

### GAP ANALYSIS: Missing "Subway Surfer" Features

- **Hoverboards / Run Savers:** A consumable item that allows the player to survive one crash. This is a critical retention and monetization feature.
- **Daily Challenges / Word Hunts:** An in-run meta-game where players collect letters to spell a word for a bonus. This drives daily engagement.
- **Mission & Quest System:** While a `MissionManager` exists, the classic "Get 1,000 coins in one run" type of mission system with scaling rewards needs to be explicitly defined and presented to the player.
- **Social Leaderboards:** Direct integration with a service like Google Play Games to show friends' high scores on the main screen.
- **Consumable Power-Up Boosts:** Allowing players to spend currency at the start of a run to enable a specific power-up (e.g., "Start with a Headstart").
- **Cinematic Finish:** [PLANNED_NOT_STARTED] A special camera sequence or animation when the player achieves a new high score or completes a significant mission.
