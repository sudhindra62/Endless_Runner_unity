
using System.Collections.Generic;
using UnityEngine;

    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

            public static event System.Action<PowerUpDefinition> OnPowerUpActivated;
            public static event System.Action<PowerUpDefinition> OnPowerUpDeactivated;
            public static event System.Action<int> OnPowerUpCollected;
            public event System.Action<PowerUp, float> OnPowerUpUpdated;
            public event System.Action<PowerUp> OnPowerUpExpired;

        private readonly List<PowerUpDefinition> _activePowerUps = new List<PowerUpDefinition>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // Iterate backwards to allow for safe removal from the list
            for (var i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                var powerUp = _activePowerUps[i];
                powerUp.Tick(Time.deltaTime);
                OnPowerUpUpdated?.Invoke(new RuntimePowerUp(powerUp), Mathf.Clamp01(powerUp.GetRemainingDuration() / Mathf.Max(powerUp.duration, 0.01f)));

                if (!powerUp.IsActive())
                {
                    OnPowerUpExpired?.Invoke(new RuntimePowerUp(powerUp));
                    DeactivatePowerUp(powerUp);
                }
            }
        }

        public void ActivatePowerUp(PowerUpDefinition powerUp)
        {
            powerUp.Activate();
            _activePowerUps.Add(powerUp);
            OnPowerUpActivated?.Invoke(powerUp);
            GameEvents.TriggerPowerUpActivated(powerUp.type.ToString());
            Logger.Log("POWER_UP_MANAGER", $"Power-up activated: {powerUp.type}");
        }

        public void DeactivatePowerUp(PowerUpDefinition powerUp)
        {
            _activePowerUps.Remove(powerUp);
            OnPowerUpDeactivated?.Invoke(powerUp);
            GameEvents.TriggerPowerUpDeactivated(powerUp.type.ToString());
            Logger.Log("POWER_UP_MANAGER", $"Power-up deactivated: {powerUp.type}");
        }

        public bool IsPowerUpActive(PowerUpType type)
        {
            return _activePowerUps.Exists(p => p.type == type);
        }

        public List<PowerUpType> GetActivePowerUpTypes()
        {
            var types = new List<PowerUpType>();
            foreach (var p in _activePowerUps) types.Add(p.type);
            return types;
        }

        public void AddShards(string powerUpType, int amount)
        {
            if (ShardInventoryManager.Instance != null)
            {
                ShardInventoryManager.Instance.AddShardsAndCheckForUnlock(powerUpType, amount);
            }
        }

        public void UnlockEffect(string effectID)
        {
            // Placeholder for effect unlocking logic
            Debug.Log($"Guardian Architect: Effect {effectID} unlocked.");
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.inventoryItemIds.Add(effectID);
                SaveManager.Instance.SaveGame();
            }
        }

        public PowerUp[] GetActivePowerUps()
        {
            var runtimePowerUps = new List<PowerUp>();
            foreach (var activePowerUp in _activePowerUps)
            {
                runtimePowerUps.Add(new RuntimePowerUp(activePowerUp));
            }

            return runtimePowerUps.ToArray();
        }

        public bool IsPowerUpActive(string powerUpID)
        {
            return _activePowerUps.Exists(p => p.type.ToString() == powerUpID);
        }

        public void CollectPowerUp(PowerUpDefinition definition)
        {
            if (definition != null) ActivatePowerUp(definition);
        }

        public void RefreshPowerUpTimer(string powerUpID, float duration)
        {
            var powerUp = _activePowerUps.Find(p => p.type.ToString() == powerUpID);
            if (powerUp != null)
            {
                powerUp.duration = duration;
                powerUp.Activate();
            }
        }

        public float GetRemainingDuration(string powerUpID)
        {
            var powerUp = _activePowerUps.Find(p => p.type.ToString() == powerUpID);
            return powerUp != null ? powerUp.GetRemainingDuration() : 0f;
        }

        public float CalculatePowerUpEffect(PowerUp powerUp, GameContext context)
        {
            if (powerUp == null) return 1f;
            // Placeholder: would calculate effect multiplier based on game context
            return 1.5f;
        }

        public void UsePowerUp(string powerUpID)
        {
            ActivatePowerUp(new PowerUpDefinition { type = System.Enum.Parse<PowerUpType>(powerUpID) });
        }

        // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

        public void ActivatePowerUp(PowerUpType type)
        {
            var def = ScriptableObject.CreateInstance<PowerUpDefinition>();
            def.type = type;
            ActivatePowerUp(def);
        }

        public void ActivatePowerUp(string powerUpTypeID)
        {
            if (System.Enum.TryParse<PowerUpType>(powerUpTypeID, out var type))
            {
                ActivatePowerUp(type);
            }
        }

        public void CollectPowerUp(string powerUpID)
        {
            // Convert string ID to type for collection
            if (System.Enum.TryParse<PowerUpType>(powerUpID, out var type))
            {
                var def = ScriptableObject.CreateInstance<PowerUpDefinition>();
                def.type = type;
                CollectPowerUp(def);
            }
        }

        public void DeactivatePowerUp(PowerUpType type)
        {
            var powerUp = _activePowerUps.Find(p => p.type == type);
            if (powerUp != null)
            {
                DeactivatePowerUp(powerUp);
            }
        }

        public void ActivatePowerUp(PowerUpType type, float duration)
        {
            var def = ScriptableObject.CreateInstance<PowerUpDefinition>();
            def.type = type;
            def.duration = duration;
            ActivatePowerUp(def);
        }

        private sealed class RuntimePowerUp : PowerUp
        {
            private readonly PowerUpDefinition definition;

            public RuntimePowerUp(PowerUpDefinition source)
            {
                definition = source;
                powerUpType = source.type;
                duration = source.GetRemainingDuration();
            }

            public PowerUpType Type => powerUpType;

            public override void ApplyEffect() { }
            public override void RemoveEffect() { }
        }
    }

