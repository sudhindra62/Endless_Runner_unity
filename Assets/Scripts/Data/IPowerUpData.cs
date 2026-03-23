using UnityEngine;

/// <summary>
/// Interface for all power-up data sources (both definitions and active instances).
/// Enables type-safe polymorphism between PowerUpDefinition and active PowerUp instances.
/// </summary>
public interface IPowerUpData
{
    PowerUpType GetPowerUpType();
    string GetPowerUpName();
    float GetDuration();
    Sprite GetIcon();
}
