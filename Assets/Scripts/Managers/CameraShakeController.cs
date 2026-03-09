using UnityEngine;
using System.Collections;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// Manages camera shake effects.
    /// This is a Singleton that can be accessed from any script to trigger a shake.
    /// It ensures that only one instance of the controller exists.
    /// </summary>
    public class CameraShakeController : MonoBehaviour
    {
        // --- SINGLETON PATTERN ---
        public static CameraShakeController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            // This manager is expected to live on a persistent "Managers" GameObject.
        }

        // --- SHAKE STATE ---
        private Coroutine _shakeCoroutine;
        
        /// <summary>
        /// The current displacement vector caused by the shake.
        /// This is read by the CameraController in LateUpdate.
        /// </summary>
        public Vector3 ShakeOffset { get; private set; } = Vector3.zero;

        /// <summary>
        /// Triggers a camera shake with a given duration and magnitude.
        /// If a shake is already in progress, it will be stopped and replaced by the new one.
        /// </summary>
        /// <param name="duration">How long the shake should last, in seconds.</param>
        /// <param name="magnitude">The intensity of the shake.</param>
        public void TriggerShake(float duration, float magnitude)
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            _shakeCoroutine = StartCoroutine(Shake(duration, magnitude));
        }

        private IEnumerator Shake(float duration, float magnitude)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Generate a random point inside a 2D circle for a more natural shake on the XY plane.
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                
                ShakeOffset = new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                yield return null; // Wait for the next frame before recalculating.
            }

            // Reset shake offset and coroutine reference once the shake is complete.
            ShakeOffset = Vector3.zero;
            _shakeCoroutine = null;
        }
    }
}
