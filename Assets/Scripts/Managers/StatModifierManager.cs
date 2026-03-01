using UnityEngine;

public class StatModifierManager : MonoBehaviour
{
    private const string SkillTreeSourceId = "SkillTree";

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
            ApplyAllModifiers();
        }
        else if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            RemoveAllModifiers();
        }
    }

    private void ApplyAllModifiers()
    {
        SkillTreeManager skillTree = SkillTreeManager.Instance;
        if (skillTree == null) return;

        // Apply PowerUpManager Modifiers
        ApplyPowerUpModifiers(skillTree);

        // TODO: Apply modifiers for other managers once they are refactored
        // ApplyScoreManagerModifiers(skillTree);
        // ApplyReviveManagerModifiers(skillTree);
        // ApplyGameDifficultyManagerModifiers(skillTree);
    }

    private void RemoveAllModifiers()
    {
        // This ensures all skill tree modifiers are cleared at the end of a run
        ServiceLocator.Get<PowerUpManager>()?.RemoveDurationModifier(PowerUpType.Shield, SkillTreeSourceId);
        ServiceLocator.Get<PowerUpManager>()?.RemoveDurationModifier(PowerUpType.Magnet, SkillTreeSourceId);
        // ... and so on for all other modifiers
    }

    private void ApplyPowerUpModifiers(SkillTreeManager skillTree)
    {
        PowerUpManager powerUpManager = ServiceLocator.Get<PowerUpManager>();
        if (powerUpManager == null) return;

        float shieldDurationBonus = skillTree.GetAggregatedModifier(ModifierType.ShieldDurationBoost);
        if (shieldDurationBonus > 0)
        {
            powerUpManager.ApplyDurationModifier(PowerUpType.Shield, SkillTreeSourceId, 1 + shieldDurationBonus);
        }

        float magnetRadiusBonus = skillTree.GetAggregatedModifier(ModifierType.MagnetRadiusBoost); // Note: MagnetRadius requires new logic
        if (magnetRadiusBonus > 0)
        {
            // This will require adding a new modifier type to the PowerUpManager for radius
            Debug.Log("Magnet Radius Boost from Skill Tree is not implemented in PowerUpManager yet.");
        }
    }
}
