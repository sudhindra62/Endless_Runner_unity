using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest") ]
public class Quest : ScriptableObject
{
    public string title;
    public string description;
    public QuestObjective[] objectives;
}
