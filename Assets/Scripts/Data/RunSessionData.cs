using UnityEngine;

[CreateAssetMenu(fileName = "RunSessionData", menuName = "Gameplay/Run Session Data")]
public class RunSessionData : ScriptableObject
{
    public float distance;
    public float styleScore;
    public int comboPeak;
    public float riskLaneUsage;
    public float duration;
    public int score;
    public int reviveCount;

    public float GetTotalDistance()
    {
        return distance;
    }
}
