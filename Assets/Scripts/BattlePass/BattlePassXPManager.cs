
using UnityEngine;

public class BattlePassXPManager : Singleton<BattlePassXPManager>
{
    public void GrantXP(int amount)
    {
        if (LiveOpsManager.Instance != null && LiveOpsManager.Instance.IsEventActive("DoubleXPWeekend"))
        {
            amount *= 2;
        }

        BattlePassManager.Instance.AddXP(amount);
        Debug.Log($"Granted {amount} Battle Pass XP.");
    }

    // Example methods for granting XP from various sources
    public void OnRunCompleted()
    {
        GrantXP(20);
    }

    public void OnMissionCompleted()
    {
        GrantXP(50);
    }

    public void OnBossDefeated()
    {
        GrantXP(100);
    }

    public void OnDailyLogin()
    {
        GrantXP(10); // A small reward for logging in
    }
}
