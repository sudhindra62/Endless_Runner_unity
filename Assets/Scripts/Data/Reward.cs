using System;
using UnityEngine;

    [System.Serializable]
    public class Reward
    {
        public string name;
        public RewardType rewardType;
        public int quantity;
        public string itemID;
        public ThemeSO temporaryTheme;
        public int temporaryThemeDurationHours;

        public Reward(string name, RewardType type, int amount, string id)
    {
            this.name = name;
            this.rewardType = type;
            this.quantity = amount;
            this.itemID = id;
        }

        public Reward() : this(string.Empty, RewardType.Coins, 0, string.Empty)
        {
        }

        public int amount { get => quantity; set => quantity = value; }
    }

