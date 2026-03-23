/// <summary>
/// Interface for all rewardable items.
/// Enables polymorphism between different reward types (coins, gems, items).
/// </summary>
public interface IRewardable
{
    string GetRewardID();
    string GetRewardName();
    int GetRewardQuantity();
    RewardType GetRewardType();
}
