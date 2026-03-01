using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RunModifierManager : MonoBehaviour
{
    public static RunModifierManager Instance { get; private set; }

    [Header("Modifier Selection Rules")]
    [SerializeField] private List<RunModifierData> allModifiers;
    [SerializeField] private int minModifiers = 1;
    [SerializeField] private int maxModifiers = 3;
    [SerializeField] private int maxTotalDifficulty = 10;

    private List<RunModifierData> activeModifiers = new List<RunModifierData>();
    private const string RunModifierSourceId = "RunModifier";

    public delegate void ModifiersUpdated(List<RunModifierData> activeModifiers);
    public static event ModifiersUpdated OnModifiersApplied;

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
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            SelectAndApplyModifiers();
        }
        else if (newState == GameState.EndOfRun)
        {
            RevertAllModifiers();
        }
    }

    private void SelectAndApplyModifiers()
    {
        activeModifiers.Clear();
        var availableModifiers = new List<RunModifierData>(allModifiers);
        int currentDifficulty = 0;
        int modifiersToSelect = Random.Range(minModifiers, maxModifiers + 1);

        for (int i = 0; i < modifiersToSelect && availableModifiers.Count > 0; i++)
        {
            var validPicks = availableModifiers.Where(mod => 
                currentDifficulty + mod.difficultyScore <= maxTotalDifficulty && 
                !activeModifiers.Any(activeMod => activeMod.incompatibleWith.Contains(mod.modifierType) || mod.incompatibleWith.Contains(activeMod.modifierType))
            ).ToList();

            if (validPicks.Count == 0) break;

            RunModifierData selected = validPicks[Random.Range(0, validPicks.Count)];
            
            activeModifiers.Add(selected);
            availableModifiers.Remove(selected);
            currentDifficulty += selected.difficultyScore;
        }

        foreach (var modifier in activeModifiers)
        {
            ApplyModifier(modifier);
        }

        OnModifiersApplied?.Invoke(activeModifiers);
        Debug.Log($"Applied {activeModifiers.Count} modifiers with a total difficulty of {currentDifficulty}");
    }

    private void ApplyModifier(RunModifierData modifier)
    {
        string sourceId = $"{RunModifierSourceId}_{modifier.modifierType}";
        switch (modifier.modifierType)
        {
            // Player Movement
            case ModifierType.SpeedMultiplier:
                ServiceLocator.Get<PlayerMovement>()?.ApplySpeedMultiplier(sourceId, modifier.modifierValue);
                break;
            case ModifierType.GravityModifier:
                ServiceLocator.Get<PlayerMovement>()?.ApplyGravityMultiplier(sourceId, modifier.modifierValue);
                break;
            case ModifierType.JumpDisabled:
                ServiceLocator.Get<PlayerMovement>()?.SetJumpDisabled(sourceId, true);
                break;
            case ModifierType.ReverseInput:
                ServiceLocator.Get<PlayerMovement>()?.SetReverseInput(sourceId, true);
                break;

            // Difficulty/Spawners
            case ModifierType.ObstacleDensityIncrease:
            case ModifierType.CoinDensityIncrease:
                ServiceLocator.Get<GameDifficultyManager>()?.ApplyDifficultyMultiplier(sourceId, modifier.modifierValue);
                break;

            // Scoring
            case ModifierType.StyleBonusMultiplier:
                ServiceLocator.Get<ScoreManager>()?.ApplyScoreMultiplier(sourceId, (int)modifier.modifierValue);
                break;
            case ModifierType.ComboTimeoutReduction:
                 ServiceLocator.Get<GameDifficultyManager>()?.ApplyDifficultyMultiplier(sourceId, modifier.modifierValue); // Assuming combo is tied to difficulty
                break;

            // Fever Mode
            case ModifierType.FeverChargeBoost:
                ServiceLocator.Get<FeverModeManager>()?.ApplyChargeMultiplier(sourceId, modifier.modifierValue);
                break;

            // Power-Ups
            case ModifierType.PowerUpDurationMultiplier:
                ServiceLocator.Get<PowerUpManager>()?.ApplyDurationMultiplier(sourceId, modifier.modifierValue);
                break;

            // Visuals
            case ModifierType.VisionFog:
                ServiceLocator.Get<VisionFogManager>()?.SetFogState(true);
                break;

            default:
                Debug.LogWarning($"Modifier type {modifier.modifierType} application not implemented.");
                break;
        }
    }

    private void RevertAllModifiers()
    {
        // Instead of targeted removal, we command each manager to reset its state,
        // which is safer and ensures no mutations are left over.
        ServiceLocator.Get<PlayerMovement>()?.ResetState();
        ServiceLocator.Get<ScoreManager>()?.ResetState();
        ServiceLocator.Get<GameDifficultyManager>()?.ResetState();
        ServiceLocator.Get<PowerUpManager>()?.ResetState();
        ServiceLocator.Get<FeverModeManager>()?.ResetState();
        ServiceLocator.Get<VisionFogManager>()?.ResetState();
        
        // Recalculate passives from other systems after reset
        ServiceLocator.Get<SkillTreeManager>()?.RecalculatePassives();
        ServiceLocator.Get<CharacterPassiveManager>()?.RecalculatePassives();

        activeModifiers.Clear();
        Debug.Log("All run modifiers have been reverted and manager states are reset.");
    }
}
