
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Core;

namespace Gameplay
{
    /// <summary>
    /// Manages the application and removal of power-up effects on the player.
    /// </summary>
    public class PowerUpEffectsController : MonoBehaviour
    {
        private readonly Dictionary<PowerUpType, Coroutine> _activeEffects = new Dictionary<PowerUpType, Coroutine>();

        /// <summary>
        /// Applies a power-up effect for a specified duration.
        /// </summary>
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
            // Add logic to apply the effect based on the effectType.
            Debug.Log($"Applied {effectType} effect.");
        }

        private void RemoveEffect(PowerUpType effectType)
        {
            // Add logic to remove the effect based on the effectType.
            Debug.Log($"Removed {effectType} effect.");
        }
    }
}
