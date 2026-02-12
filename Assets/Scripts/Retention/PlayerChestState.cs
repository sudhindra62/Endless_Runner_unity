using System;

/// <summary>
/// Represents the state of a single chest instance in the player's inventory.
/// This class is serializable to allow for saving and loading player progress.
/// </summary>
[Serializable]
public class PlayerChestState
{
    /// <summary>
    /// The type of the chest, linking to its ChestData definition.
    /// </summary>
    public ChestType Type;

    /// <summary>
    /// The current status of the chest (e.g., Locked, Unlocking, ReadyToClaim).
    /// </summary>
    public ChestStatus Status = ChestStatus.Locked;

    /// <summary>
    /// The timestamp (in UTC) when the chest began unlocking.
    /// Stored as a string to ensure easy serialization.
    /// </summary>
    public string UnlockStartTimeUtc;

    /// <summary>
    /// A flag to indicate if this is a new chest that hasn't been seen by the player.
    /// </summary>
    public bool IsNew = true;

    // Public property to get the UnlockStartTime as a DateTime object.
    public DateTime UnlockStartTime
    {
        get
        {
            if (DateTime.TryParse(UnlockStartTimeUtc, out DateTime result))
            {
                return result;
            }
            return DateTime.UtcNow;
        }
        set => UnlockStartTimeUtc = value.ToString("o"); // ISO 8601 format
    }
}

/// <summary>
/// Defines the possible states of a chest in the player's inventory.
/// </summary>
[Serializable]
public enum ChestStatus
{
    /// <summary>
    /// The chest is locked and waiting for an unlock timer to start.
    /// </summary>
    Locked,
    
    /// <summary>
    /// The chest is currently in the process of unlocking.
    /// </summary>
    Unlocking,
    
    /// <summary>
    /// The chest has finished unlocking and is ready to be opened.
    /// </summary>
    ReadyToClaim
}