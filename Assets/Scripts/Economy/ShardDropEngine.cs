
using System;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Data;
using EndlessRunner.AI;
using EndlessRunner.Security;

namespace EndlessRunner.Economy
{
    /// <summary>
    /// A static engine responsible for calculating and awarding shard drops from enemies.
    /// Integrates with PityCounterManager and DropIntegrityValidator.
    /// </summary>
    public static class ShardDropEngine
    {
        private static readonly System.Random random = new System.Random();

        private const double LegendaryDropRate = 0.005;
        private const double RareDropRate = 0.05;
        private const double UncommonDropRate = 0.20;
        private const double CommonDropRate = 0.50;

        public static void CalculateAndAwardShard(EnemyAIType enemyType)
        { 
            if (PityCounterManager.Instance.HasPityThresholdBeenMet())
            {
                Debug.LogWarning("SHARD_DROP_ENGINE: Pity threshold met! Awarding GUARANTEED Legendary Shard.");
                AwardShard(ShardType.Legendary, 1, true);
                return;
            }

            double roll = random.NextDouble();
            double bonusModifier = GetBonusModifier(enemyType);

            ShardType? droppedShard = null;
            bool wasLegendary = false;

            if (roll < LegendaryDropRate + (bonusModifier * 0.1))
            {
                droppedShard = ShardType.Legendary;
                wasLegendary = true;
            }
            else if (roll < RareDropRate + (bonusModifier * 0.2))
            {
                droppedShard = ShardType.Rare;
            }
            else if (roll < UncommonDropRate + bonusModifier)
            {
                droppedShard = ShardType.Uncommon;
            }
            else if (roll < CommonDropRate + bonusModifier * 2)
            {
                droppedShard = ShardType.Common;
            }

            if (droppedShard.HasValue)
            {
                AwardShard(droppedShard.Value, 1, wasLegendary);
            }
            else
            { 
                PityCounterManager.Instance.IncrementPityCounter();
            }
        }

        private static double GetBonusModifier(EnemyAIType enemyType)
        {
            switch (enemyType)
            {
                case EnemyAIType.Advanced: return 0.05;
                case EnemyAIType.Basic: return 0.01;
                default: return 0.0;
            }
        }

        private static void AwardShard(ShardType type, int quantity, bool wasLegendary)
        {
            // --- INTEGRITY VALIDATION STEP ---
            string salt = DateTime.UtcNow.Ticks.ToString();
            string serverHash = DropIntegrityValidator.GenerateDropHash(type, salt);
            bool isDropValid = DropIntegrityValidator.ValidateDrop(type, salt, serverHash);

            if (!isDropValid)
            {
                Debug.LogError($"SHARD_DROP_ENGINE: Drop validation FAILED for {type}. Aborting award.");
                return; // Do not award the shard if validation fails.
            }
            Debug.Log($"SHARD_DROP_ENGINE: Drop validation PASSED for {type}.");
            // --- END VALIDATION ---

            var inventory = DataManager.Instance.GameData.GetShardInventory();
            
            if (inventory.ContainsKey(type))
            {
                inventory[type] += quantity;
            }
            else
            {
                inventory[type] = quantity;
            }
            
            DataManager.Instance.GameData.SetShardInventory(inventory);
            DataManager.Instance.SaveData();

            Debug.Log($"SHARD_DROP_ENGINE: Player awarded {quantity}x {type} Shard!");

            if (wasLegendary)
            {
                PityCounterManager.Instance.ResetPityCounter();
            }
            else
            {
                PityCounterManager.Instance.IncrementPityCounter();
            }

            if(UIManager.Instance != null)
            {
                UIManager.Instance.UpdateShardCountUI();
            }
        }
    }
}
