using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject questPanel;
    public Text questTitle;
    public Text questDescription;

    public void UpdateQuestUI(Quest quest)
    {
        questTitle.text = quest.title;
        questDescription.text = quest.description;
    }
}
