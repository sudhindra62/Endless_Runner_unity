
using System;
using UnityEngine;

    [Serializable]
    public class AchievementData
    {
        public string id;
        public string name;
        public string description;
        public int currentProgress;
        public bool isUnlocked;
        public string achievementName; // ADDED: UI field alias
        public Sprite icon; // ADDED: Missing UI display field
        public string iconReference;
        public string tier; // ADDED: Achievement tier
        public int requiredValue; // ADDED: Target progress value

        // --- Property Aliases for Architectural Sync (Folder 2) ---
        public string Name => name;
        public string Description => description;

        public AchievementData(string achievementId)
        {
            id = achievementId;
            currentProgress = 0;
            isUnlocked = false;
        }

        public AchievementData()
        {
            id = string.Empty;
            currentProgress = 0;
            isUnlocked = false;
        }
    }

