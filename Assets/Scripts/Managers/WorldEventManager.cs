
using System;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public static WorldEventManager Instance { get; private set; }

    public static event Action<WorldEventData> OnEventActivated;
    public static event Action OnEventDeactivated;

    private WorldEventData currentEvent;
    private bool isEventActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FetchAndValidateEvent();
    }

    private void FetchAndValidateEvent()
    {
        string eventJson = RemoteConfig.GetString("activeWorldEvent");
        if (string.IsNullOrEmpty(eventJson)) 
        {
            DeactivateEvent();
            return;
        }

        WorldEventData eventData = JsonUtility.FromJson<WorldEventData>(eventJson);

        // Validate Time
        DateTime now = DateTime.UtcNow; // Use server time if available
        if (now >= eventData.startTime && now < eventData.endTime)
        {
            ActivateEvent(eventData);
        }
        else
        {
            DeactivateEvent();
        }
    }

    private void ActivateEvent(WorldEventData eventData)
    {
        if (isEventActive && currentEvent.eventId == eventData.eventId) return;

        currentEvent = eventData;
        isEventActive = true;

        ApplyEventModifiers(currentEvent);
        ApplyVisualTheme(currentEvent.visualThemeId);

        OnEventActivated?.Invoke(currentEvent);
        Debug.Log($"World Event Activated: {currentEvent.eventName}");
    }

    public void DeactivateEvent()
    {
        if (!isEventActive) return;

        RemoveEventModifiers(currentEvent);
        RemoveVisualTheme();

        Debug.Log($"World Event Deactivated: {currentEvent.eventName}");
        currentEvent = null;
        isEventActive = false;
        OnEventDeactivated?.Invoke();
    }

    private void ApplyEventModifiers(WorldEventData eventData)
    {
        foreach (var modifier in eventData.modifierTypes)
        {
            switch (modifier)
            {
                case WorldEventType.DoubleCoins:
                    DataManager.Instance.ApplyCoinMultiplier(currentEvent.eventId, currentEvent.rewardBoost.coinMultiplier);
                    break;
                case WorldEventType.DoubleGems:
                    DataManager.Instance.ApplyGemMultiplier(currentEvent.eventId, 2f); // Example: Or use a value from rewardBoost
                    break;
                case WorldEventType.SpeedFestival:
                    PlayerMovement.Instance.ApplySpeedMultiplier(currentEvent.eventId, 1.2f);
                    GameDifficultyManager.Instance.ApplyDifficultyMultiplier(currentEvent.eventId, 1.2f);
                    break;
                case WorldEventType.RareDropBoost:
                    RareDropManager.Instance.SetProbabilityBonus(currentEvent.rewardBoost.rareDropChanceBonus);
                    break;
                case WorldEventType.BossRush:
                    BossChaseManager.Instance.SetCooldownMultiplier(0.5f);
                    break;
                case WorldEventType.GravityShift:
                    PlayerMovement.Instance.ApplyGravityMultiplier(currentEvent.eventId, 0.8f);
                    break;
                case WorldEventType.ZombieMode:
                    ZombieManager.Instance.ActivateZombieMode();
                    break;
                case WorldEventType.DarkVisionMode:
                    VisionFogManager.Instance.ActivateDarkVision(0.7f); // Example intensity
                    break;
                case WorldEventType.ComboFestival:
                    FlowComboManager.Instance.SetComboWindowMultiplier(1.5f); // 50% larger window
                    break;
                case WorldEventType.NoPowerUpsChallenge:
                    PowerUpManager.Instance.SetPowerUpsDisabled(true);
                    break;
            }
        }
        DataManager.Instance.ApplyXpMultiplier(currentEvent.eventId, currentEvent.rewardBoost.xpMultiplier);
        LeagueManager.Instance.ApplyLeaguePointsMultiplier(currentEvent.rewardBoost.leaguePointsMultiplier);
    }

    private void RemoveEventModifiers(WorldEventData eventData)
    {
        foreach (var modifier in eventData.modifierTypes)
        {
            switch (modifier)
            {
                case WorldEventType.DoubleCoins:
                    DataManager.Instance.RemoveCoinMultiplier(currentEvent.eventId);
                    break;
                case WorldEventType.DoubleGems:
                    DataManager.Instance.RemoveGemMultiplier(currentEvent.eventId);
                    break;
                case WorldEventType.SpeedFestival:
                    PlayerMovement.Instance.RemoveSpeedMultiplier(currentEvent.eventId);
                    GameDifficultyManager.Instance.RemoveDifficultyMultiplier(currentEvent.eventId);
                    break;
                case WorldEventType.RareDropBoost:
                    RareDropManager.Instance.SetProbabilityBonus(0f);
                    break;
                case WorldEventType.BossRush:
                    BossChaseManager.Instance.SetCooldownMultiplier(1f);
                    break;
                case WorldEventType.GravityShift:
                    PlayerMovement.Instance.RemoveGravityMultiplier(currentEvent.eventId);
                    break;
                case WorldEventType.ZombieMode:
                    ZombieManager.Instance.DeactivateZombieMode();
                    break;
                case WorldEventType.DarkVisionMode:
                    VisionFogManager.Instance.DeactivateDarkVision();
                    break;
                case WorldEventType.ComboFestival:
                    FlowComboManager.Instance.SetComboWindowMultiplier(1f);
                    break;
                case WorldEventType.NoPowerUpsChallenge:
                    PowerUpManager.Instance.SetPowerUpsDisabled(false);
                    break;
            }
        }
        DataManager.Instance.RemoveXpMultiplier(currentEvent.eventId);
        LeagueManager.Instance.RemoveLeaguePointsMultiplier();
    }

    private void ApplyVisualTheme(string themeId)
    {
        if (string.IsNullOrEmpty(themeId)) return;
        ThemeManager.Instance.ApplyTheme(themeId);
    }

    private void RemoveVisualTheme()
    {
        if (currentEvent != null && !string.IsNullOrEmpty(currentEvent.visualThemeId))
        {
            ThemeManager.Instance.RevertToDefaultTheme();
        }
    }
}


