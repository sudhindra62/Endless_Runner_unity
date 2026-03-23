using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Central type converter for resolving type mismatches across game systems.
/// Provides safe conversion between related types (e.g., PowerUpDefinition → PowerUpType, Reward → RewardItem).
/// Phase 2A (Type Consistency) implementation.
/// </summary>
public static class TypeConverter
{
    // --- PowerUp System Type Converters ---

    public static PowerUpType GetPowerUpType(PowerUpDefinition definition)
    {
        return definition != null ? definition.type : PowerUpType.None;
    }

    public static PowerUpDefinition GetPowerUpDefinition(string powerUpTypeID)
    {
        // Placeholder: would load from database by type
        return null;
    }

    public static string GetPowerUpID(PowerUpDefinition definition)
    {
        return definition != null ? definition.type.ToString() : "";
    }

    // --- Reward System Type Converters ---

    public static RewardType GetRewardType(Reward reward)
    {
        return reward != null ? reward.rewardType : RewardType.Coins;
    }

    public static string GetRewardID(Reward reward)
    {
        return reward != null ? reward.itemID : "";
    }

    public static RewardItem RewardToRewardItem(Reward reward)
    {
        if (reward == null) return new RewardItem();
        return new RewardItem
        {
            itemID = reward.itemID,
            quantity = reward.quantity,
            type = reward.rewardType,
            description = reward.name
        };
    }

    public static Reward RewardItemToReward(RewardItem item)
    {
        return new Reward(item.description, item.type, item.quantity, item.itemID);
    }

    public static int GetRewardQuantity(Reward reward)
    {
        return reward != null ? reward.quantity : 0;
    }

    // --- Skin System Type Converters ---

    public static string GetSkinIdentifier(SkinData skin)
    {
        return skin != null ? skin.skinID : "";
    }

    public static SkinData GetSkinByIdentifier(string skinID, List<SkinData> skinDatabase)
    {
        if (skinDatabase == null) return null;
        return skinDatabase.Find(s => s.skinID == skinID);
    }

    public static int GetSkinCost(SkinData skin)
    {
        return skin != null ? skin.price : 0;
    }

    // --- Theme System Type Converters ---

    public static string GetThemeIdentifier(ThemeSO theme)
    {
        return theme != null ? theme.themeName : "";
    }

    public static ThemeSO GetThemeByIdentifier(string themeID, ThemeSO[] themeDatabase)
    {
        if (themeDatabase == null) return null;
        foreach (var theme in themeDatabase)
        {
            if (theme.themeName == themeID) return theme;
        }
        return null;
    }

    public static int GetThemeIndex(string themeID, ThemeSO[] themeDatabase)
    {
        if (themeDatabase == null) return -1;
        for (int i = 0; i < themeDatabase.Length; i++)
        {
            if (themeDatabase[i].themeName == themeID) return i;
        }
        return -1;
    }

    // --- League/Rank System Type Converters ---

    public static LeagueTier IntToLeagueTier(int rankValue)
    {
        return (LeagueTier)Mathf.Clamp(rankValue, 0, (int)LeagueTier.Diamond);
    }

    public static int LeagueTierToInt(LeagueTier tier)
    {
        return (int)tier;
    }

    public static string LeagueTierToString(LeagueTier tier)
    {
        return tier.ToString();
    }

    public static LeagueTier StringToLeagueTier(string rankName)
    {
        if (System.Enum.TryParse<LeagueTier>(rankName, out var result))
            return result;
        return LeagueTier.Bronze;
    }

    // --- Quest System Type Converters ---

    public static string GetQuestIdentifier(QuestData quest)
    {
        return quest != null ? quest.ID : "";
    }

    public static QuestData GetQuestByIdentifier(string questID, QuestData[] questDatabase)
    {
        if (questDatabase == null) return null;
        foreach (var quest in questDatabase)
        {
            if (quest.questID == questID) return quest;
            if (quest.ID == questID) return quest;
        }
        return null;
    }
}
