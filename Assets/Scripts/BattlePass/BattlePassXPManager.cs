using UnityEngine;

public class BattlePassXPManager : Singleton<BattlePassXPManager>
{
    [SerializeField] private int runCompletedXP = 50;
    [SerializeField] private int missionCompletedXP = 75;
    [SerializeField] private int bossDefeatedXP = 100;

    public void OnRunCompleted()
    {
        BattlePassManager.Instance?.AddXP(runCompletedXP);
    }

    public void OnMissionCompleted()
    {
        BattlePassManager.Instance?.AddXP(missionCompletedXP);
    }

    public void OnBossDefeated()
    {
        BattlePassManager.Instance?.AddXP(bossDefeatedXP);
    }
}
