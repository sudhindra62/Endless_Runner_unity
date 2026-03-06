using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class AchievementManagerTest
{
    private AchievementManager achievementManager;
    private GameObject managerObject;

    [SetUp]
    public void SetUp()
    {
        managerObject = new GameObject();
        achievementManager = managerObject.AddComponent<AchievementManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(managerObject);
    }

    [UnityTest]
    public IEnumerator UnlockAchievement_AwardsReward()
    {
        // Find a specific achievement to test
        Achievement distanceAchievement = achievementManager.GetAchievementData().achievements[0];

        // Mock the RewardManager
        GameObject rewardManagerObject = new GameObject();
        RewardManager rewardManager = rewardManagerObject.AddComponent<RewardManager>();

        bool rewardAwarded = false;
        string awardedItemId = "";
        int awardedQuantity = 0;

        RewardManager.Instance.OnRewardAwarded += (id, quantity) =>
        {
            rewardAwarded = true;
            awardedItemId = id;
            awardedQuantity = quantity;
        };

        // Directly update progress to unlock the achievement
        achievementManager.progressTracker.UpdateProgress(distanceAchievement.type, distanceAchievement.goal);

        // Wait a frame for the event to propagate
        yield return null;

        Assert.IsTrue(rewardAwarded, "Reward was not awarded.");
        Assert.AreEqual(distanceAchievement.reward.itemID, awardedItemId, "Awarded item ID is incorrect.");
        Assert.AreEqual(distanceAchievement.reward.quantity, awardedQuantity, "Awarded quantity is incorrect.");

        Object.Destroy(rewardManagerObject);
    }
}
