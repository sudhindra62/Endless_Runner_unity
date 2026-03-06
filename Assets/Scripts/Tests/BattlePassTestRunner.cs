
using UnityEngine;

public class BattlePassTestRunner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("--- STARTING BATTLE PASS TEST ---");

        // Test 1: Player earns XP
        BattlePassXPManager.Instance.OnRunCompleted();
        BattlePassXPManager.Instance.OnMissionCompleted();
        Debug.Log("[PASS] XP earned.");

        // Test 2: Unlock a level
        for(int i = 0; i < 5; i++)
        {
            BattlePassXPManager.Instance.OnBossDefeated(); // 100 XP each
        }
        Debug.Assert(BattlePassManager.Instance.GetBattlePassData().currentLevel > 1, "[FAIL] Did not level up.");
        Debug.Log("[PASS] Leveled up.");

        // Test 3: Claim a reward
        BattlePassManager.Instance.ClaimReward(1);
        Debug.Log("[PASS] Claimed a reward.");

        // Test 4: Buy premium pass
        BattlePassManager.Instance.PurchasePremiumPass();
        Debug.Assert(BattlePassManager.Instance.GetBattlePassData().hasPremiumPass, "[FAIL] Premium pass not purchased.");
        Debug.Log("[PASS] Purchased premium pass.");

        // Test 5: Season Reset (Manual Trigger for Test)
        BattlePassManager.Instance.TriggerSeasonResetForTesting();
        Debug.Assert(BattlePassManager.Instance.GetBattlePassData().currentLevel == 1, "[FAIL] Season did not reset.");
        Debug.Assert(BattlePassManager.Instance.GetBattlePassData().currentXP == 0, "[FAIL] Season did not reset.");
        Debug.Log("[PASS] Season reset.");

        Debug.Log("--- BATTLE PASS TEST COMPLETE ---");
    }
}
