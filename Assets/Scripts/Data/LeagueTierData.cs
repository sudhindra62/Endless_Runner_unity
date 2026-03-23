using System.Collections.Generic;

[System.Serializable]
public struct LeagueReward
{
    public RewardType rewardType;
    public int amount;

    public LeagueReward(RewardType rewardType, int amount)
    {
        this.rewardType = rewardType;
        this.amount = amount;
    }
}

[System.Serializable]
public struct LeagueTierData
{
    public int promotionScore;
    public int scoreThreshold;
    public List<LeagueReward> rewards;

    public LeagueTierData(int promotionScore, int scoreThreshold, List<LeagueReward> rewards)
    {
        this.promotionScore = promotionScore;
        this.scoreThreshold = scoreThreshold;
        this.rewards = rewards;
    }
}
