using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the process of fusing power-ups. 
/// Monitored by ScriptableObject definitions for scalability and event-driven for system decoupling.
/// Global scope Singleton.
/// </summary>
public class FusionModifierData
{
    public FusionType Type { get; }
    public float Duration { get; }
    public FusionModifierData(FusionType type, float duration) { Type = type; Duration = duration; }
}

public class PowerUpFusionManager : Singleton<PowerUpFusionManager>
{
    public event Action<FusionModifierData> OnFusionActivated;
    public event Action OnFusionDeactivated;

    [Header("Fusion Configuration")]
    [SerializeField] private GameObject fusionCorePrefab;
    private List<PowerUpFusionDefinition> fusionDefinitions;
    
    private FusionType currentFusion = FusionType.None;
    private float fusionTimer;

    protected override void Awake()
    {
        base.Awake();
        LoadFusionDefinitions();
    }

    private void Start()
    {
        PowerUpManager.OnPowerUpActivated += HandlePowerUpActivated;
        PowerUpManager.OnPowerUpDeactivated += HandlePowerUpDeactivated;
    }

    private void OnDestroy()
    {
        PowerUpManager.OnPowerUpActivated -= HandlePowerUpActivated;
        PowerUpManager.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
    }

    private void Update()
    {
        if (currentFusion != FusionType.None)
        {
            fusionTimer -= Time.deltaTime;
            if (fusionTimer <= 0) DeactivateFusion();
        }
    }

    private void LoadFusionDefinitions()
    {
        fusionDefinitions = Resources.LoadAll<PowerUpFusionDefinition>("PowerUpFusions").ToList();
    }

    private void HandlePowerUpActivated(PowerUpDefinition definition) => CheckFusions();
    private void HandlePowerUpDeactivated(PowerUpDefinition definition) => CheckFusions();

    private void CheckFusions()
    {
        if (currentFusion != FusionType.None || PowerUpManager.Instance == null) return;

        HashSet<PowerUpType> activePowerUps = new HashSet<PowerUpType>(PowerUpManager.Instance.GetActivePowerUpTypes());
        foreach (var fusion in fusionDefinitions)
        {
            if (activePowerUps.Contains(fusion.powerUp1) && activePowerUps.Contains(fusion.powerUp2))
            {
                TriggerFusion(fusion);
                break;
            }
        }
    }

    private void TriggerFusion(PowerUpFusionDefinition fusion)
    {
        currentFusion = ConvertTypeToFusionEnum(fusion.fusedPowerUp);
        fusionTimer = fusion.fusedDuration;

        PowerUpManager.Instance.DeactivatePowerUp(fusion.powerUp1);
        PowerUpManager.Instance.DeactivatePowerUp(fusion.powerUp2);
        PowerUpManager.Instance.ActivatePowerUp(fusion.fusedPowerUp, fusion.fusedDuration);

        OnFusionActivated?.Invoke(new FusionModifierData(currentFusion, fusionTimer));

        if (fusionCorePrefab != null && PlayerController.Instance != null)
        {
            Instantiate(fusionCorePrefab, PlayerController.Instance.transform.position, Quaternion.identity);
        }
    }

    private void DeactivateFusion()
    {
        currentFusion = FusionType.None;
        fusionTimer = 0;
        OnFusionDeactivated?.Invoke();
    }

    private FusionType ConvertTypeToFusionEnum(PowerUpType type)
    {
        return type switch
        {
            PowerUpType.CoinStorm => FusionType.CoinStorm,
            PowerUpType.InvincibleDash => FusionType.InvincibleDash,
            PowerUpType.FeverFrenzy => FusionType.FeverFrenzy,
            _ => FusionType.None
        };
    }
}
