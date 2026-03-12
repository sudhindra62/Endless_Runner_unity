
using System;
using UnityEngine;
using Core;

namespace Managers
{
    /// <summary>
    /// The central authority for the player character's state, health, and interactions.
    /// It integrates with the GameManager for game state changes and the PowerUpManager for effects.
    /// This script was rewritten from a stub by Supreme Guardian Architect v13 to be a core architectural pillar.
    /// </summary>
    public class PlayerManager : Singleton<PlayerManager>
    {
        // --- EVENTS ---
        public event Action<int> OnHealthChanged;
        public event Action OnPlayerDeath;

        // --- PUBLIC PROPERTIES ---
        public bool IsInvincible { get; private set; }
        public int CurrentHealth { get; private set; }

        // --- CONFIGURATION ---
        [Header("Player Stats")]
        [SerializeField] private int maxHealth = 3;

        [Header("Component References")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator playerAnimator;

        [Header("Effects")]
        [Tooltip("Visual effect to enable when the player is invincible.")]
        [SerializeField] private GameObject invincibilityEffect;

        // --- UNITY LIFECYCLE & EVENT WIRING ---

        protected override void Awake()
        {
            base.Awake();
            CurrentHealth = maxHealth;
            if (playerController == null) playerController = GetComponentInChildren<PlayerController>();
            if (playerAnimator == null) playerAnimator = GetComponentInChildren<Animator>();
            SubscribeToEvents();
        }

        private void Start()
        {
            // Set initial state
            SetInvincibility(false);
            OnHealthChanged?.Invoke(CurrentHealth);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        // --- PUBLIC API ---

        /// <summary>
        /// Applies damage to the player. If health drops to 0, triggers the death sequence.
        /// </summary>
        public void TakeDamage(int amount)
        {
            if (IsInvincible) 
            {
                Debug.Log("Guardian Architect: Player took damage but is invincible. Ignoring.");
                return; // A-TO-Z CONNECTIVITY: Power-up system is now integrated here.
            }
            
            if (CurrentHealth <= 0) return; // Already dead

            CurrentHealth -= amount;
            OnHealthChanged?.Invoke(CurrentHealth);
            Debug.Log($"Guardian Architect: Player took {amount} damage. Health is now {CurrentHealth}");

            // You might trigger a hit animation or sound effect here
            // playerAnimator.SetTrigger("Hit");

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Restores player health to full and resets state for a new game.
        /// </summary>
        public void ReviveAndReset()
        {
            CurrentHealth = maxHealth;
            playerController.enabled = true;
            // playerAnimator.SetBool("IsDead", false);
            OnHealthChanged?.Invoke(CurrentHealth);
            Debug.Log("Guardian Architect: Player has been revived and reset.");
        }
        
        // --- PRIVATE LOGIC ---

        /// <summary>
        /// Handles the player's death sequence.
        /// </summary>
        private void Die()
        {
            CurrentHealth = 0;
            playerController.enabled = false; // Disable player input/movement
            // playerAnimator.SetBool("IsDead", true);

            Debug.Log("Guardian Architect: Player has died. Firing OnPlayerDeath event.");
            OnPlayerDeath?.Invoke();

            // Tell the GameManager that the game is over.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
            }
        }

        /// <summary>
        /// Toggles the invincibility state and its associated visual effect.
        /// </summary>
        private void SetInvincibility(bool isInvincible)
        {
            IsInvincible = isInvincible;
            if (invincibilityEffect != null)
            {
                invincibilityEffect.SetActive(isInvincible);
            }
            Debug.Log($"Guardian Architect: Player invincibility set to {isInvincible}");
        }

        // --- EVENT HANDLERS (A-TO-Z CONNECTIVITY) ---

        private void SubscribeToEvents()
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
            }
        }

        private void HandlePowerUpActivated(PowerUpDefinition powerUpDef)
        {
            if (powerUpDef.type == PowerUpType.Invincibility)
            {
                SetInvincibility(true);
            }
        }

        private void HandlePowerUpDeactivated(PowerUpType powerUpType)
        {
            if (powerUpType == PowerUpType.Invincibility)
            {
                SetInvincibility(false);
            }
        }
    }
}
