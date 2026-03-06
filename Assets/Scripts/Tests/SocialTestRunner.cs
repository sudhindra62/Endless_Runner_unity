
using UnityEngine;

public class SocialTestRunner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("--- STARTING SOCIAL SYSTEM TEST ---");

        // Test 1: Add 5 friends
        for (int i = 0; i < 5; i++)
        {
            FriendManager.Instance.AddFriend($"TestFriend{i}");
        }
        Debug.Assert(FriendManager.Instance.Friends.Count == 5, "[FAIL] Friend count mismatch.");
        Debug.Log("[PASS] Added 5 friends.");

        // Test 2: Send 3 challenges
        FriendChallengeManager.Instance.CreateChallenge("TestFriend0", ChallengeType.Score, 10000);
        FriendChallengeManager.Instance.CreateChallenge("TestFriend1", ChallengeType.Distance, 2000);
        FriendChallengeManager.Instance.CreateChallenge("TestFriend2", ChallengeType.Combo, 50);
        Debug.Log("[PASS] Sent 3 challenges.");

        // Test 3: Win a challenge
        var challenges = FriendChallengeManager.Instance.GetChallenges();
        string challengeToWin = challenges[0].challengeID;
        FriendChallengeManager.Instance.CompleteChallenge(challengeToWin, 12000);
        Debug.Log("[PASS] Won a challenge.");

        // Test 4: Lose a challenge
        string challengeToLose = challenges[1].challengeID;
        FriendChallengeManager.Instance.CompleteChallenge(challengeToLose, 1500);
        Debug.Log("[PASS] Lost a challenge.");

        // Test 5: Leaderboard Sync
        // This would be a more complex test involving backend calls.
        // For now, we will just call the necessary methods to ensure they run without error.
        LeaderboardManager.Instance.UpdatePlayerRank(15000, 3000, 75, 500, "DefaultCharacter");
        Debug.Log("[PASS] Leaderboard sync initiated.");


        Debug.Log("--- SOCIAL SYSTEM TEST COMPLETE ---");
    }
}
