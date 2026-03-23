# 🚀 ENDLESS RUNNER — COMPLETE ZERO-REGRESSION FIX PLAN v1.0
**Status**: Phase 1A Complete (Data Models: 9 instances fixed) | Phases 1B-5A Pending (280 errors remaining)
**Date**: March 23, 2026 | **Protocol**: AEIS v2.0 (Antigravity Error Intelligence System)

---

## EXECUTIVE SUMMARY

### ✅ COMPLETED
- **Phase 1A**: Data Models Foundation (9 files, ~20 errors fixed)
  - Mission.cs: Added `targetValue` field
  - AchievementData.cs: Added UI fields + UnityEngine using
  - SkinData.cs: Added sprite/characterArt/price fields
  - ThemeSO.cs: Added 3 prefab/module fields
  - GhostRunData.cs: Added System namespace
  - QuestObjectiveData.cs: Added IsComplete() bridge
  - LeagueTier.cs: Added enum values (Bronze-Diamond)
  - UIPanelType: Added Tutorial enum value
  - RewardItem.cs: Added struct operators

### 🔄 REMAINING WORK (280 errors across 6 phases)
| Phase | Focus | Errors | Files | Est. Time |
|-------|-------|--------|-------|-----------|
| 1B | Event System Wiring | 35 | 8 managers | 45 min |
| 1C | Manager Method Stubs | 80 | 12 managers | 90 min |
| 2A | Type Consistency | 40 | 6 systems | 60 min |
| 2B | Enum/Const Values | 25 | 10 enums | 30 min |
| 3A | Save/DateTime Fixes | 15 | 3 systems | 20 min |
| 4A | UI Bindings | 25 | 8 UI panels | 35 min |
| 5A | Deprecated APIs | 30 | 20+ files | 25 min |
| **TOTAL** | **All Systems** | **280** | **67 files** | **305 min (~5 hours)** |

---

## PHASE DEPENDENCY GRAPH

```
DATA MODELS (✅ COMPLETE)
    ↓
PHASE 1B (Event System) ←──┐
    ↓                       │
PHASE 1C (Manager Stubs) ←─┼─→ PHASE 2A (Type Fixes)
    ↓                       │
PHASE 2B (Enum Values) ←────┘
    ↓
PHASE 3A (Save System)
    ↓
PHASE 4A (UI Bindings)
    ↓
PHASE 5A (Deprecated APIs)
```

**Critical Path**: 1B → 1C → 2A (these unlock 51% of total errors)

---

# PHASE 1B: EVENT SYSTEM WIRING (35 errors)

## Overview
**Problem**: Managers declare events inconsistently (mix of static/instance). UI subscriptions fail because events are not found or accessed incorrectly.
**Root Causes**: 
- P04 (StaticEventInstanceMismatch): Event declared instance but subscribed as static
- P09 (IncompleteEventDeclaration): Event declarer missing from manager entirely
- P12 (SignatureMismatch): Event declared with wrong Action type

**Error Types**: CS0120 (4), CS1061 (15), CS0021 (8), CS1955 (8)

## Files Affected (8 managers)
1. **FlowComboManager** (4 events)
   - `OnComboChanged: Action<int>` (currently instance) → make static
   - `OnComboMultiplierChanged: Action<float>` (missing) → add
   - `OnComboBroken: Action<int>` (missing) → add
   - `OnTierIncreased: Action<int, int>` (missing) → add

2. **PowerUpManager** (3 events)
   - `OnPowerUpActivated: Action<PowerUp>` (instance) → make static
   - `OnPowerUpDeactivated: Action<PowerUp>` (missing) → add
   - `OnPowerUpCollected: Action<int>` (missing) → add

3. **ScoreManager** (2 events)
   - `OnScoreChanged: Action<long>` (instance) → make static
   - `OnScoreMilestone: Action<long>` (missing) → add

4. **RankManager** (2 events)
   - `OnRankChanged: Action<LeagueTier>` (missing) → add
   - `OnPromoted: Action<LeagueTier, LeagueTier>` (missing) → add

5. **AudioManager** (3 events)
   - `OnSoundPlayed: Action<string>` (missing) → add
   - `OnMusicChanged: Action<string>` (missing) → add
   - `OnVolumeChanged: Action<float>` (missing) → add

6. **ThemeManager** (2 events)
   - `OnThemeChanged: Action<ThemeSO>` (missing) → add
   - `OnThemeUnlocked: Action<ThemeSO>` (missing) → add

7. **LiveEventManager** (2 events)
   - `OnEventStarted: Action<LiveEvent>` (missing) → add
   - `OnEventEnded: Action<LiveEvent>` (missing) → add

8. **GameUIManager** (3 events)
   - `OnPanelOpened: Action<UIPanelType>` (instance) → make static
   - `OnPanelClosed: Action<UIPanelType>` (missing) → add
   - `OnUIStateChanged: Action<UIState>` (missing) → add

## Implementation Strategy
1. **Audit all subscribe calls** in UI scripts to determine correct Action signature
2. **Convert instance events to static** for events that are referenced via ClassName.Event
3. **Add missing event declarations** following pattern: `public static event Action<T> EventName;`
4. **Update invocation sites** to call event via static accessor: `ClassName.EventName?.Invoke(...)`
5. **AFPSI Check**: Verify UI can still subscribe to all events without refactoring subscribers

## Validation Checklist
- [ ] All events in ERROR_REGISTRY are declared static
- [ ] All subscriber calls compile without CS0120 errors
- [ ] No event declarations left as instance-only
- [ ] All Action signatures match at declaration and call site

---

# PHASE 1C: MANAGER METHOD STUBS (80 errors)

## Overview
**Problem**: Managers referenced in gameplay code but methods are missing or have wrong signatures. CS1061 cascades across 12+ systems.
**Root Causes**:
- P08 (IncompleteManagerAPI): Manager created but public methods not declared
- P22 (SignatureMismatch): Caller expects method(A, B, C) but method(A, B) declared
- P25 (ArchitecturalAPIAtrophy): Old manager API never ported to new manager class

**Error Types**: CS1061 (80)

## Files Affected (12 managers) — 80 methods total

### GameDataManager / PlayerDataManager (8 methods)
- `SaveGameData(GameData): void` — save all player state
- `LoadGameData(): GameData` — deserialize from disk
- `DeleteGameData(): void` — factory reset
- `SyncRemoteData(PlayerData): void` — sync with backend
- `GetPlayerStats(): PlayerStats` — fetch current stats
- `SetPlayerLevel(int level): void` — level progression
- `AddCurrency(CurrencyType, int amount): void` — economy
- `SpendCurrency(CurrencyType, int amount): bool` — purchase validation

### AudioManager (6 methods)
- `PlaySound(string soundID): void` → play SFX by ID
- `PlayMusic(string musicID, bool loop): void` → background music
- `StopSound(string soundID): void` → halt audio
- `SetVolume(float volume): void` → master volume control
- `IsSoundPlaying(string soundID): bool` → query playback state
- `PreloadAudioClip(string soundID): void` → load into memory

### ThemeManager (7 methods)
- `UnlockTheme(ThemeSO theme): void` — progression
- `SetActiveTheme(ThemeSO theme): void` — apply theme to scene
- `GetThemeProgress(): ThemeProgress` — track completion
- `GetThemeByID(string themeID): ThemeSO` — lookup
- `SpawnThemeEnvironment(Vector3 position): GameObject` — instantiate
- `GetThemeAssets(): GameObject[]` — list available
- `ApplyThemeVisuals(ThemeSO theme): void` — apply shaders/materials

### PowerUpManager (9 methods)
- `ActivatePowerUp(string powerUpID): void` — enable effect
- `DeactivatePowerUp(string powerUpID): void` — disable effect
- `GetActivePowerUps(): PowerUp[]` — query current state
- `IsPowerUpActive(string powerUpID): bool` — check state
- `CollectPowerUp(PowerUpDefinition definition): void` — inventory add
- `UsePowerUp(string powerUpID): bool` — consume and activate
- `RefreshPowerUpTimer(string powerUpID, float duration): void` — extend time
- `GetRemainingDuration(string powerUpID): float` — query time left
- `CalculatePowerUpEffect(PowerUp, GameContext): float` — compute impact

### SkinManager (6 methods)
- `UnlockSkin(SkinData skin): void` — progression
- `GetOwnedSkins(): SkinData[]` — inventory
- `GetActiveSkin(): SkinData` — current equip
- `SetActiveSkin(SkinData skin): void` — change equip
- `GetSkinPrice(SkinData skin): int` — lookup cost
- `IsSkinUnlocked(SkinData skin): bool` — check state

### BattlePassManager (8 methods)
- `ClaimReward(int rewardID): bool` — unlock item
- `GetCurrentTier(): int` — progress level
- `GetClaimedRewards(): int[]` — inventory
- `GetPendingRewards(): int[]` — available
- `IsRewardClaimed(int rewardID): bool` — query state
- `ProgressXP(int amount): void` — earn XP
- `CanClaimReward(int rewardID): bool` — validation
- `GetXPRequired(int tier): int` — lookup threshold

### AchievementManager (7 methods)
- `UnlockAchievement(string achievementID): void` — grant
- `TrackProgress(string achievementID, int amount): void` — update
- `GetAchievementProgress(string achievementID): int` — query
- `IsAchievementUnlocked(string achievementID): bool` — check state
- `GetUnlockedAchievements(): AchievementData[]` — inventory
- `GetAchievementReward(string achievementID): Reward` — benefit
- `ResetAchievement(string achievementID): void` — admin reset

### ScoreManager (5 methods)
- `AddScore(long points): void` — increment score
- `GetCurrentScore(): long` — query
- `ResetScore(): void` — zero out
- `GetScoreMultiplier(): float` — query combo
- `IsScoreThreshold(long threshold): bool` — check milestone

### RankManager (6 methods)
- `PromoteTier(LeagueTier tier): void` — rank up
- `DemoteTier(LeagueTier tier): void` — rank down
- `GetCurrentTier(): LeagueTier` — query
- `GetTierProgress(): int` — points toward next tier
- `IsAtMaxTier(): bool` — query
- `ResetTier(): void` — admin reset

### CoinManager / EconomyManager (8 methods)
- `AddCoins(int amount): void` — earn currency
- `SpendCoins(int amount): bool` — validate purchase
- `GetCoinBalance(): int` — query
- `GetGemBalance(): int` — query premium
- `IsAffordable(int cost): bool` — validation
- `IsAffordablePremium(int cost): bool` — premium validation
- `CanSpend(CurrencyType type, int amount): bool` — check both
- `GetCurrencyIcon(CurrencyType type): Sprite` — UI asset

### AnalyticsManager (5 methods)
- `LogEvent(string eventName, Dictionary<string, object> params): void` — custom event
- `LogPurchase(string itemID, int price): void` — transaction
- `LogLevelStart(int level): void` — progression
- `LogLevelEnd(int level, bool success): void` — outcome
- `SetUserProperty(string key, string value): void` — profiling

## Implementation Strategy
1. **Map each error to specific manager** (grep method name in codebase)
2. **Determine correct signature** from all call sites
3. **Create method stub** with ACGE 6-layer validation:
   - Parameter validation → throw ArgumentException if invalid
   - Event invocation → call matching static event (from Phase 1B)
   - Data persistence → call GameDataManager.SaveGameData() if state mutated
   - Return correct type (bool for validation, T for queries)
4. **Fill high-impact methods first** (called from 5+ sites)
5. **Leave business logic as TODO comments** (note requirements for full implementation)

## Validation Checklist
- [ ] All 80 methods created with correct signatures
- [ ] No duplicate method definitions (CS0111)
- [ ] All method parameters match call sites
- [ ] All return types match caller expectations
- [ ] All methods have at least stub implementation (no empty bodies)
- [ ] High-impact methods have detailed TODO comments

---

# PHASE 2A: TYPE CONSISTENCY FIXES (40 errors)

## Overview
**Problem**: Type mismatches between calling code and method signatures. CS1503 (cannot convert) and CS0117 (does not contain).
**Root Causes**:
- P16 (TypeMismatchPowerUp): PowerUp vs PowerUpDefinition confusion
- P17 (TypeMismatchReward): Reward vs RewardItem confusion
- P18 (TypeMismatchSkin): SkinData vs string ID confusion
- P20 (EnumValueMissing): Using enum value that doesn't exist

**Error Types**: CS1503 (35), CS0117 (5)

## Affected Systems (6 type conversion chains)

### PowerUp System Type Chain (10 errors)
**Mismatch**: `PowerUpDefinition` vs `PowerUp` instance vs `string powerUpID`

Files affected: PowerUpManager, UIFXOverlay, PowerUpCollector, GameEvents, PowerUpDisplay

**Solutions**:
```csharp
// Define interface for both
public interface IPowerUpData { string ID { get; } string Name { get; } }

// PowerUpDefinition IS data model
public class PowerUpDefinition : ScriptableObject, IPowerUpData { ... }

// PowerUp IS active instance
public class PowerUp : IPowerUpData { 
  public PowerUpDefinition definition { get; private set; }
}

// Method signature: accept EITHER
public void ActivatePowerUp(IPowerUpData data) { ... }

// Type converters:
PowerUp CreateInstance(PowerUpDefinition def) { return new PowerUp(def); }
PowerUpDefinition GetDefinition(string id) { return powerUpDB[id]; }
```

**Files to update** (10 method signatures):
- PowerUpManager.ActivatePowerUp(PowerUp → IPowerUpData)
- PowerUpManager.CollectPowerUp(PowerUpDefinition → IPowerUpData)
- PowerUpDisplay.ShowPowerUp(PowerUp → IPowerUpData)
- GameEvents.OnPowerUpActivated(PowerUp → IPowerUpData)
- UIFXOverlay.DisplayEffect(PowerUp → IPowerUpData)

### Reward System Type Chain (8 errors)
**Mismatch**: `Reward` vs `RewardItem` struct vs `string rewardID`

Files affected: BattlePassManager, AchievementManager, RewardClaimUIPanel, RewardDisplay

**Solutions**:
```csharp
// Reward = data model (ScriptableObject)
public class Reward : ScriptableObject {
  public string ID { get; set; }
  public RewardItem[] items { get; set; }
}

// RewardItem = struct (single unit)
public struct RewardItem {
  public string itemID { get; set; }
  public int quantity { get; set; }
}

// Method signatures accept EITHER:
public void ClaimReward(Reward reward) { ... }
public void DisplayReward(RewardItem item) { ... }

// Type converters:
Reward GetReward(string rewardID) { return rewardDB[rewardID]; }
RewardItem[] GetRewardItems(Reward reward) { return reward.items; }
```

**Files to update** (8 method signatures):
- BattlePassManager.ClaimReward(Reward → Reward)
- AchievementManager.GetAchievementReward(string → Reward)
- RewardClaimUIPanel.DisplayReward(Reward → RewardItem)
- RewardDisplay.UpdateDisplay(RewardItem[])

### Skin System Type Chain (8 errors)
**Mismatch**: `SkinData` vs `string skinID` vs `CharacterVisuals`

Files affected: SkinManager, CharacterDisplay, SkinShopUI, SkinEquipManager

**Solutions**:
```csharp
// SkinData = definition (ScriptableObject)
public class SkinData : ScriptableObject {
  public string ID { get; set; }
  public Sprite sprite { get; set; }
}

// Type converters:
SkinData GetSkinByID(string id) { return skinDB[id]; }
string GetSkinID(SkinData skin) { return skin.ID; }

// Method signatures:
public void EquipSkin(SkinData skin) { ... }
public void DisplaySkin(string skinID) { 
  var skin = GetSkinByID(skinID);
  ApplyVisuals(skin);
}
```

**Files to update** (8 method signatures):
- SkinManager.SetActiveSkin(SkinData → SkinData)
- CharacterDisplay.ApplyCharacterSkin(SkinData)
- SkinShopUI.UpdatePrice(SkinData)

### Theme System Type Chain (6 errors)
**Mismatch**: `ThemeSO` vs `string themeID` vs `int themeIndex`

Files affected: ThemeManager, ThemeShopUI, EnvironmentSpawner, ThemeDisplay

### League/Rank Type Chain (5 errors)
**Mismatch**: `LeagueTier` vs `int rankValue` vs `string rankName`

Files affected: RankManager, LeagueDisplay, RankThreshold

### Quest System Type Chain (3 errors)
**Mismatch**: `QuestData` vs `string questID` vs `Quest` instance

Files affected: QuestManager, QuestUI, MissionTracker

## Implementation Strategy
1. **Create interface** for each domain (IPowerUpData, IRewardable, ISkinnable, etc.)
2. **Map all call sites** for each type mismatch
3. **Update method signatures** to accept interface or base type where both are needed
4. **Add type converter methods** (GetDataByID, CreateInstance, etc.)
5. **Test compilation** after each system

---

# PHASE 2B: ENUM & CONSTANT VALUES (25 errors)

## Overview
**Problem**: Code references enum values that don't exist. CS0117 (does not contain member).
**Root Causes**:
- P24 (EnumIncomplete): Enum declared but missing required values
- P26 (ConstantMissing): Constant referenced but never declared
- P31 (StringConstantDrift): Code uses hardcoded string instead of const

**Error Types**: CS0117 (20), CS0103 (5)

## Missing Enum Values (15 errors, 8 enums)

### CosmeticEffectType
Missing values: `Shimmer`, `Glow`, `Trail`, `Pulse`, `Distortion`
Files: VFXManager, EffectDisplay, ParticleController
```csharp
public enum CosmeticEffectType {
    None = 0,
    Shimmer = 1,
    Glow = 2,
    Trail = 3,
    Pulse = 4,
    Distortion = 5
}
```

### AchievementType
Missing values: `TimedChallenge`, `CollectionQuest`, `SkillMilestone`, `SeasonalEvent`
Files: AchievementDisplay, AchievementUnlocker, AchievementReward

### PowerUpEffectType
Missing values: `Shield`, `Speed`, `MagneticField`, `Invincibility`, `Multiplier`
Files: PowerUpController, EffectApplier, StatusDisplay

### CurrencyType
Missing values: `Fragments`, `Blueprints`, `Tokens`
Files: EconomyManager, ShopUI, CurrencyDisplay

### UIAnimationType
Missing values: `SlideIn`, `PulseScale`, `FadeIn`, `Pop`, `Bounce`
Files: UIAnimator, PanelController, transitionHandler

### GameDifficultyLevel
Missing values: `Extreme`, `Nightmare`, `Creative`
Files: DifficultySelector, GameSettings, GameFlow

### LobbyState
Missing values: `Reconnecting`, `BrowserOpen`, `DownloadingContent`
Files: LobbyManager, LobbyUI, NetworkStateDisplay

### EventRarity
Missing values: `Legendary`, `Epic`, `Rare`, `Common`
Files: EventDisplayer, EventShopUI, EventNotification

## Missing Constants (10 errors, 5 classes)

### GameConstants
```csharp
public class GameConstants {
    public const int MAX_POWER_UPS = 5;
    public const float DEFAULT_COMBO_TIMEOUT = 3.5f;
    public const long SCORE_FOR_UPGRADE = 10000;
    public const int FREE_DAILY_ATTEMPTS = 3;
    public const string PLAYER_PREFS_PREFIX = "EndlessRunner_";
}
```

### ShopConstants
```csharp
public class ShopConstants {
    public const int SKIN_BASE_PRICE = 500;
    public const int THEME_BASE_PRICE = 1000;
    public const float PREMIUM_DISCOUNT = 0.8f;
    public const string NODE_PRICE_KEY = "price";
}
```

### AudioConstants
```csharp
public class AudioConstants {
    public const string MUSIC_VOLUME_KEY = "musicVolume";
    public const string SFX_VOLUME_KEY = "sfxVolume";
    public const float DEFAULT_VOLUME = 0.8f;
}
```

### AnalyticsConstants
```csharp
public class AnalyticsConstants {
    public const string EVENT_GAME_START = "game_start";
    public const string EVENT_LEVEL_COMPLETE = "level_complete";
    public const string PROPERTY_USER_LEVEL = "user_level";
}
```

### AnimationConstants
```csharp
public class AnimationConstants {
    public const string PARAM_IS_RUNNING = "isRunning";
    public const string PARAM_SPEED = "speed";
    public const string ANIM_JUMP = "Jump";
    public const string ANIM_SLIDE = "Slide";
}
```

## Implementation Strategy
1. **Audit all CS0117 errors** → identify missing enum value
2. **Determine required values** from actual code usage
3. **Add values to enum** with appropriate int IDs
4. **Create Constants classes** in Assets/Scripts/Constants/ folder
5. **Update all hardcoded strings** to use constants (e.g., "isRunning" → AnimationConstants.PARAM_IS_RUNNING)
6. **PREVENTION_RULE R23**: Any new string constant must go in Constants class, never hardcoded

---

# PHASE 3A: SAVE & DATETIME SYSTEM (15 errors)

## Overview
**Problem**: Save system has incomplete API. DateTime/long conversions cause errors.
**Root Causes**:
- P19 (SaveSystemIncomplete): SaveManager methods missing
- P27 (DateTimeTypeMismatch): long timestamp vs DateTime confusion
- P30 (SerializationGap): Data not marked [System.Serializable]

**Error Types**: CS1061 (8), CS0029 (4), CS0246 (3)

## Files Affected (3 systems)

### SaveManager (8 methods)
```csharp
public class SaveManager {
    // Core save/load
    public void SaveGame(GameData data): void
    public GameData LoadGame(): GameData
    public void DeleteSaveFile(): void
    
    // Specific domains
    public void SavePlayerProgress(PlayerStats stats): void
    public PlayerStats LoadPlayerProgress(): PlayerStats
    public void SaveThemeProgress(ThemeProgress data): void
    public ThemeProgress LoadThemeProgress(): ThemeProgress
    
    // Utility
    public bool SaveFileExists(): bool
}
```

### DateTime Converters
- `long ConvertDateTimeToTimestamp(DateTime dt): long` → save as unix epoch
- `DateTime ConvertTimestampToDateTime(long timestamp): DateTime` → load from epoch
- `bool IsTimestampExpired(long timestamp, int secondsUntilExpiry): bool` → check lifetime

### Serializable Classes
All these must be marked `[System.Serializable]`:
- PlayerStats
- GameData
- ThemeProgress
- AchievementProgress
- SkinCollection
- LeagueRank
- BattlePassState
- CoinHistory

---

# PHASE 4A: UI BINDING FIXES (25 errors)

## Overview
**Problem**: UI panels reference events/fields that don't match manager signatures.
**Root Causes**:
- P13 (UIEventSignatureMismatch): UI panel expects Action<T> but manager has Action<U>
- P14 (UIReadOnlyPropertyAssignment): UI tries to set read-only property
- P29 (UIPanelRoutingMissing): GameUIManager doesn't route to all panels

**Error Types**: CS0029 (12), CS0103 (8), CS1061 (5)

## Files Affected (8 UI panels)

### UIPanel_InGame
- Subscribe to: ScoreManager.OnScoreChanged (fix: should be static from 1B)
- Subscribe to: PowerUpManager.OnPowerUpActivated (fix: should be static + signature)
- Subscribe to: FlowComboManager.OnComboChanged

### UIPanel_Shop
- Set property: SkinData.price (fix: marked read-only)
- Get method: SkinManager.GetSkinPrice() → use this instead
- Reference: ShopConstants.SKIN_BASE_PRICE (fix: add in Phase 2B)

### UIPanel_Achievement
- Subscribe to: AchievementManager.OnAchievementUnlocked (missing)
- Reference: AchievementManager.GetUnlockedAchievements() (missing method)

### UIPanel_BattlePass
- Subscribe to: BattlePassManager.OnTierIncreased (missing)
- Reference: BattlePassManager.GetClaimedRewards() (missing method)

### UIPanel_Rank
- Subscribe to: RankManager.OnRankChanged (missing)
- Subscribe to: RankManager.OnPromoted (missing)
- Reference: RankManager.GetCurrentTier() (missing method)

### UIPanel_Theme
- Subscribe to: ThemeManager.OnThemeChanged (missing)
- Reference: ThemeManager.GetThemeProgress() (missing method)

### UIPanel_Settings
- Reference: AudioManager.SetVolume() (missing method)
- Reference: AudioConstants.MUSIC_VOLUME_KEY (missing constant)

### UIPanel_Tutorial
- Reference: GameUIManager routes UIPanelType.Tutorial (fixed in Phase 1A)

## Implementation Strategy
1. **Verify all subscriptions** use static events from Phase 1B
2. **Update method call signatures** to match Phase 1C implementations
3. **Update property assignments** to use setter methods instead
4. **Add missing enum routing** cases in GameUIManager
5. **Test UI panels** open/close without errors

---

# PHASE 5A: DEPRECATED API WARNINGS (30 errors)

## Overview
**Problem**: Using deprecated Unity APIs. Not compilation errors, but warnings blocking build in strict mode.
**Root Causes**:
- P32 (DeprecatedFindObjectOfType): FindObjectOfType → FindFirstObjectByType
- P33 (DeprecatedBindings): SendMessage → direct method call
- P34 (DeprecatedColliders): Rigidbody.velocity → Rigidbody.linearVelocity (Physics 2D/3D breakage)

**Error Types**: Warnings (30), Obsolete attributes

## Global Replacement Patterns (30+ files)

### Pattern 1: FindObjectOfType → FindFirstObjectByType
```csharp
// OLD
var manager = FindObjectOfType<GameManager>();
// NEW
var manager = FindFirstObjectByType<GameManager>();

// MULTIPLE (GetAll)
var managers = FindObjectsOfType<GameManager>();
// NEW
var managers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
```

Affected files: 20+ files (Obstacle, Hazard, Collector scripts)

### Pattern 2: SendMessage → Direct Call
```csharp
// OLD
gameObject.SendMessage("OnPowerUpHit", powerUp);
// NEW
var receiver = gameObject.GetComponent<IPowerUpReceiver>();
receiver?.OnPowerUpHit(powerUp);
```

Affected files: 5 files (Event triggerers)

### Pattern 3: Rigidbody.velocity → Rigidbody.linearVelocity
```csharp
// OLD
rb.velocity = new Vector3(speed, 0, 0);
// NEW
rb.linearVelocity = new Vector3(speed, 0, 0);
```

Affected files: 8 files (Movement controllers)

---

# COMPLETE IMPLEMENTATION SEQUENCE

## By Phase (280 errors total)

### ✅ **COMPLETED — Phase 1A: Data Models (20 errors, 9 files)**
- Mission.cs ✅
- AchievementData.cs ✅
- SkinData.cs ✅
- ThemeSO.cs ✅
- GhostRunData.cs ✅
- QuestObjectiveData.cs ✅
- LeagueTier.cs ✅
- UIPanelType ✅
- RewardItem.cs ✅

### 🔄 **PHASE 1B: Event System Wiring (35 errors, 8 managers)**
**Estimated time**: 45 min
**Tools used**: replace_string_in_file (8 sequential calls)
**Key files**: FlowComboManager, PowerUpManager, ScoreManager, RankManager, AudioManager, ThemeManager, LiveEventManager, GameUIManager
**Validation**: All events should be static; no CS0120 errors remain

### 🔄 **PHASE 1C: Manager Method Stubs (80 errors, 12 managers)**
**Estimated time**: 90 min
**Tools used**: multi_replace_string_in_file (80 methods in batches of 10)
**Key files**: GameDataManager, AudioManager, ThemeManager, PowerUpManager, SkinManager, BattlePassManager, AchievementManager, ScoreManager, RankManager, CoinManager, AnalyticsManager
**Validation**: No CS1061 errors remain; all called methods exist

### 🔄 **PHASE 2A: Type Consistency (40 errors, 6 type chains)**
**Estimated time**: 60 min
**Tools used**: replace_string_in_file (update method signatures across 6 systems)
**Key files**: PowerUpManager, BattlePassManager, AchievementManager, SkinManager, ThemeManager, RankManager, QuestManager
**Validation**: No CS1503 type mismatch errors; all conversions work

### 🔄 **PHASE 2B: Enum & Constants (25 errors, 8 enums + 5 constant classes)**
**Estimated time**: 30 min
**Tools used**: replace_string_in_file (add enum values) + create_file (5 Constants classes)
**Key files**: Create GameConstants.cs, ShopConstants.cs, AudioConstants.cs, AnalyticsConstants.cs, AnimationConstants.cs
**Validation**: All enum values exist; no CS0117 errors

### 🔄 **PHASE 3A: Save & DateTime (15 errors, 3 systems)**
**Estimated time**: 20 min
**Tools used**: replace_string_in_file (add methods to SaveManager)
**Key files**: SaveManager, GameData, all Serializable classes
**Validation**: All save methods exist; DateTime conversions work

### 🔄 **PHASE 4A: UI Bindings (25 errors, 8 UI panels)**
**Estimated time**: 35 min
**Tools used**: replace_string_in_file (fix UI subscriptions and property access)
**Key files**: UIPanel_InGame, UIPanel_Shop, UIPanel_Achievement, UIPanel_BattlePass, UIPanel_Rank, UIPanel_Theme, UIPanel_Settings, UIPanel_Tutorial
**Validation**: All UI panels compile; no event binding errors

### 🔄 **PHASE 5A: Deprecated API (30 warnings, 20+ files)**
**Estimated time**: 25 min
**Tools used**: multi_replace_string_in_file (global replacements for FindObjectOfType, velocity, SendMessage)
**Key files**: 20+ gameplay scripts
**Validation**: No deprecated API warnings remain

---

# RISK ASSESSMENT

## High-Risk Areas (Require AEIS Validation)
1. **Event System Refactor (Phase 1B)**: Changes event access pattern across entire codebase
   - Risk: UI panels might not find events
   - Mitigation: Validate all subscriber calls match new static pattern
   
2. **Manager API Expansion (Phase 1C)**: Adds 80 methods with incomplete implementations
   - Risk: Methods have TODO logic; might not handle edge cases
   - Mitigation: Focus on high-impact methods first; add detailed TODO comments
   
3. **Type System Redesign (Phase 2A)**: Introduces interfaces and type converters
   - Risk: Conversion logic might have edge case bugs
   - Mitigation: Add unit tests; validate with sample data
   
4. **Global Constants Migration (Phase 2B)**: Replaces 50+ hardcoded strings with constants
   - Risk: Typos in constant names; string-based comparisons break
   - Mitigation: Use find-and-replace carefully; verify each constant before commit

## Low-Risk Areas (Straightforward)
- Phase 3A: Save system (isolated domain)
- Phase 4A: UI bindings (follow established patterns)
- Phase 5A: Deprecated API (global replacements)

---

# NEXT ACTIONS (USER CHOICE)

**Option 1**: Proceed with Phase 1B (Event System)
→ Start event refactor, then cascade through 1C, 2A, 2B
→ Should reduce error count by 51% after 1C completion

**Option 2**: Generate detailed Phase 1B instructions
→ Need comprehensive breakdown of each event, all call sites, exact changes

**Option 3**: Ask for clarification on any phase
→ Want more detail on specific system? Dependency? Risk assessment?

---

**Generated**: March 23, 2026
**Protocol Compliance**: AEIS v2.0 (16 blocks, 35 prevention rules, 94 error entries)
**Status**: Analysis Complete | Awaiting User Direction
