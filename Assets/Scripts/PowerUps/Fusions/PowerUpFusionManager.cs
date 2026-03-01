using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the process of fusing power-ups. It monitors active power-ups and triggers fusions
/// when the right conditions are met, as defined by PowerUpFusionDefinition ScriptableObjects.
/// </summary>
public class PowerUpFusionManager : MonoBehaviour
{
    [Header("Fusion Configuration")]
    [Tooltip("The prefab to spawn when a fusion occurs.")]
    [SerializeField] private GameObject fusionCorePrefab;

    private PowerUpManager powerUpManager;
    private List<PowerUpFusionDefinition> fusionDefinitions;

    private void Start()
    {
        powerUpManager = ServiceLocator.Get<PowerUpManager>();
        if (powerUpManager == null)
        {
            Debug.LogError("PowerUpManager not found. Fusion system will not operate.");
            return;
        }

        LoadFusionDefinitions();
        powerUpManager.OnPowerUpActivated += CheckFusions;
        powerUpManager.OnPowerUpDeactivated += CheckFusions;
    }

    private void OnDestroy()
    {
        if (powerUpManager != null)
        {
            powerUpManager.OnPowerUpActivated -= CheckFusions;
            powerUpManager.OnPowerUpDeactivated -= CheckFusions;
        }
    }

    /// <summary>
    /// Loads all PowerUpFusionDefinition assets from the Resources folder.
    /// </summary>
    private void LoadFusionDefinitions()
    {
        fusionDefinitions = Resources.LoadAll<PowerUpFusionDefinition>("PowerUpFusions").ToList();
        if (fusionDefinitions.Count == 0)
        {
            Debug.LogWarning("No power-up fusion definitions found in Resources/PowerUpFusions.");
        }
    }

    /// <summary>
    /// Checks the currently active power-ups against all known fusion definitions.
    /// </summary>
    private void CheckFusions(PowerUpType changedType)
    {
        HashSet<PowerUpType> activePowerUps = powerUpManager.GetActivePowerUpTypes();

        foreach (var fusion in fusionDefinitions)
        {
            if (activePowerUps.Contains(fusion.powerUp1) && activePowerUps.Contains(fusion.powerUp2))
            {
                TriggerFusion(fusion);
                break; // Assume only one fusion can happen at a time
            }
        }
    }

    /// <summary>
    /// Triggers a fusion. This deactivates the base power-ups and activates the fused one.
    /// </summary>
    private void TriggerFusion(PowerUpFusionDefinition fusion)
    {
        Debug.Log($"Fusion triggered! {fusion.powerUp1} + {fusion.powerUp2} = {fusion.fusedPowerUp}");

        // Deactivate the base power-ups
        powerUpManager.DeactivatePowerUp(fusion.powerUp1);
        powerUpManager.DeactivatePowerUp(fusion.powerUp2);

        // Activate the new, fused power-up
        powerUpManager.ActivatePowerUp(fusion.fusedPowerUp, fusion.fusedDuration);

        // Spawn the fusion visual effect
        if (fusionCorePrefab != null)
        {
            // Assuming the player is the natural spawn point
            PlayerController player = ServiceLocator.Get<PlayerController>();
            if (player != null)
            {
                Instantiate(fusionCorePrefab, player.transform.position, Quaternion.identity);
            }
        }
    }
}
