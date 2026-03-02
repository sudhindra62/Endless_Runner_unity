using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float baseGravity = -19.62f; // Doubled for a weightier feel
    [SerializeField] private float baseJumpForce = 8f;

    [Header("Caps")]
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float minSpeed = 5f;

    // Multiplier Dictionaries
    private readonly Dictionary<string, float> speedMultipliers = new Dictionary<string, float>();
    private readonly Dictionary<string, float> gravityMultipliers = new Dictionary<string, float>();
    private readonly Dictionary<string, float> jumpForceMultipliers = new Dictionary<string, float>();

    // State Dictionaries (to handle multiple sources)
    private readonly Dictionary<string, bool> jumpDisabledSources = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> reverseInputSources = new Dictionary<string, bool>();

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerMovement>();
    }

    public float GetCurrentSpeed() => CalculateValue(baseSpeed, speedMultipliers, minSpeed, maxSpeed);
    public float GetCurrentGravity() => CalculateValue(baseGravity, gravityMultipliers);
    public float GetCurrentJumpForce() => CalculateValue(baseJumpForce, jumpForceMultipliers);
    public bool IsJumpDisabled() => jumpDisabledSources.Any(s => s.Value);
    public bool IsInputReversed() => reverseInputSources.Any(s => s.Value);

    private float CalculateValue(float baseValue, Dictionary<string, float> multipliers, float min = 0, float max = float.MaxValue)
    {
        float finalValue = baseValue;
        foreach (var multiplier in multipliers.Values)
        {
            finalValue *= multiplier;
        }
        return Mathf.Clamp(finalValue, min, max);
    }

    // --- Public Methods for Modifiers --- //

    public void ApplySpeedMultiplier(string sourceId, float multiplier) => speedMultipliers[sourceId] = multiplier;
    public void RemoveSpeedMultiplier(string sourceId) => speedMultipliers.Remove(sourceId);

    public void ApplyGravityMultiplier(string sourceId, float multiplier) => gravityMultipliers[sourceId] = multiplier;
    public void RemoveGravityMultiplier(string sourceId) => gravityMultipliers.Remove(sourceId);
    
    public void ApplyJumpForceMultiplier(string sourceId, float multiplier) => jumpForceMultipliers[sourceId] = multiplier;
    public void RemoveJumpForceMultiplier(string sourceId) => jumpForceMultipliers.Remove(sourceId);

    public void SetJumpDisabled(string sourceId, bool isDisabled) => jumpDisabledSources[sourceId] = isDisabled;
    public void RemoveJumpDisabled(string sourceId) => jumpDisabledSources.Remove(sourceId);

    public void SetReverseInput(string sourceId, bool isReversed) => reverseInputSources[sourceId] = isReversed;
    public void RemoveReverseInput(string sourceId) => reverseInputSources.Remove(sourceId);

    public void ResetState()
    {
        speedMultipliers.Clear();
        gravityMultipliers.Clear();
        jumpForceMultipliers.Clear();
        jumpDisabledSources.Clear();
        reverseInputSources.Clear();
    }
}
