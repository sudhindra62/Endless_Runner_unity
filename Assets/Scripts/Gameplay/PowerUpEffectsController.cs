
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffectsController : MonoBehaviour
{
    private readonly Dictionary<PowerUpType, Coroutine> _activeEffects = new Dictionary<PowerUpType, Coroutine>();

    public void ApplyEffect(PowerUpType effectType, float duration)
    {
        if (_activeEffects.ContainsKey(effectType))
        {
            StopCoroutine(_activeEffects[effectType]);
        }

        Coroutine effectCoroutine = StartCoroutine(EffectCoroutine(effectType, duration));
        _activeEffects[effectType] = effectCoroutine;
    }

    private IEnumerator EffectCoroutine(PowerUpType effectType, float duration)
    {
        ApplyEffect(effectType);
        yield return new WaitForSeconds(duration);
        RemoveEffect(effectType);

        _activeEffects.Remove(effectType);
    }

    private void ApplyEffect(PowerUpType effectType)
    {
        Debug.Log($"Applied {effectType} effect.");
    }

    private void RemoveEffect(PowerUpType effectType)
    {
        Debug.Log($"Removed {effectType} effect.");
    }
}
