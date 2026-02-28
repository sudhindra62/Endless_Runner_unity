using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Game/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string description;
    public int reward;
}