using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Game/ChestData")]
public class ChestData : ScriptableObject
{
    public string chestName;
    public int cost;
    public Sprite chestIcon;
    public GameObject chestPrefab;
}