using UnityEngine;

public class KillObjective : QuestObjective
{
    public Enemy enemyToKill;
    public int amountToKill;
    private int amountKilled = 0;

    public void EnemyKilled()
    {
        amountKilled++;
    }

    public override bool IsComplete()
    {
        return amountKilled >= amountToKill;
    }
}
