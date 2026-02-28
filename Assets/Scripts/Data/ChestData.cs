using UnityEngine;

[CreateAssetMenu(fileName = "NewChestData", menuName = "Data/ChestData")]
public class ChestData : ScriptableObject
{
    public string chestId;
    public string chestName;
    public int coinReward;
    public float cooldownHours;
}
