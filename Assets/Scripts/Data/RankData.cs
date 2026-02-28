using UnityEngine;

[CreateAssetMenu(fileName = "NewRank", menuName = "Data/Rank")]
public class RankData : ScriptableObject
{
    public string rankName;
    public int levelRequirement;
}
