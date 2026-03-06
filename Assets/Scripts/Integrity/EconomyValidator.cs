using UnityEngine;

/// <summary>
/// Validates economic activities like currency transactions and rare drops.
/// </summary>
public class EconomyValidator
{
    /// <summary>
    /// Validates a currency transaction.
    /// </summary>
    /// <param name="currentBalance">The player's balance *before* the transaction.</param>
    /// <param name="amountChanged">The amount being added (positive) or subtracted (negative).</param>
    /// <param name="currencyType">The type of currency being changed.</param>
    /// <returns>True if the transaction is valid, false otherwise.</returns>
    public bool ValidateCurrencyTransaction(int currentBalance, int amountChanged, string currencyType)
    {
        // Rule: Final balance must not be negative.
        if ((currentBalance + amountChanged) < 0)
        {
            IntegrityManager.Instance.ReportError($"Currency validation failed. Attempted to set {currencyType} to a negative value.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Validates a rare drop against its defined probability.
    /// </summary>
    /// <param name="dropData">The data of the item that was dropped.</param>
    /// <param name="pityCounter">The current pity counter for this drop type.</param>
    /// <returns>True if the drop is plausible, false otherwise.</returns>
    public bool ValidateRareDrop(RareDropData dropData, int pityCounter)
    {
        // This is a simplified check. A production system would be more sophisticated.
        // It checks if a drop occurred at a chance far below its base probability without a significant pity boost.
        float effectiveChance = dropData.baseChance + (pityCounter * dropData.pityIncrease);

        // If the roll succeeded with an effective chance that is still extremely low (e.g., < 0.1%),
        // it might be suspicious, but it's not definitively cheating. True validation would require server-side checks.
        // For now, we'll just log if the probability seems unusually low for a successful drop.
        // This check is more for monitoring than for strict enforcement.
        if (effectiveChance < 0.001f) // Example threshold
        {
             IntegrityManager.Instance.ReportError($"Rare drop {dropData.dropName} occurred with an unusually low effective probability: {effectiveChance}");
        }

        // Rule: Drop probability cannot exceed 100%.
        if (effectiveChance > 1.0f)
        {
            IntegrityManager.Instance.ReportError($"Drop probability for {dropData.dropName} exceeded 100% ({effectiveChance}).");
            return false;
        }

        return true;
    }
}
