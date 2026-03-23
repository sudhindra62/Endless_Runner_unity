
using System;
using UnityEngine;

    /// <summary>
    /// Detects when the player is close to an obstacle and triggers a near-miss event.
    /// This component is designed to be lightweight and work with object pooling systems.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class NearMissDetector : MonoBehaviour
    {
        /// <summary>
        /// Event fired when a potential near-miss is detected.
        /// Parameters: Obstacle Instance ID, Obstacle Position, Proximity to player.
        /// </summary>
        public static event Action<int, Vector3, float> OnNearMissCandidate;

        [Header("Detection Settings")]
        [Tooltip("The layer the player character is on.")]
        [SerializeField] private LayerMask playerLayer;

        [Tooltip("If true, the trigger will be disabled after firing to prevent re-triggering.")]
        [SerializeField] private bool disableOnTrigger = true;

        private Collider _obstacleCollider;
        private Collider _triggerCollider;
        private bool _hasBeenTriggered;

        private void Awake()
        {
            _triggerCollider = GetComponent<Collider>();
            _obstacleCollider = GetComponentInParent<Collider>();

            if (_obstacleCollider == null)
            {
                Debug.LogError("NearMissDetector could not find a parent obstacle collider.", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            // Reset the trigger state when the object is enabled from a pool.
            _hasBeenTriggered = false;
            if (_triggerCollider != null)
            {
                _triggerCollider.enabled = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasBeenTriggered || ((1 << other.gameObject.layer) & playerLayer) == 0)
            {
                return;
            }

            _hasBeenTriggered = true;

            float proximity = Vector3.Distance(transform.position, other.transform.position);
            OnNearMissCandidate?.Invoke(_obstacleCollider.GetInstanceID(), _obstacleCollider.transform.position, proximity);

            if (disableOnTrigger)
            {
                _triggerCollider.enabled = false;
            }
        }
    }

