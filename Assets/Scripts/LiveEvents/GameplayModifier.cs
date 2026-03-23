using System;

[Serializable]
public class GameplayModifier
{
    public string TargetManager; // e.g., "DifficultyManager", "ScoreManager"
    public string ModifierField; // e.g., "DifficultyMultiplier"
    public float Value;
}
