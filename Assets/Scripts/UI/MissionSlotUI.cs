
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionProgressText;
    [SerializeField] private Slider missionProgressBar;

    public void SetMission(MissionData mission)
    {
        missionNameText.text = mission.missionName;
        UpdateProgress(mission);
    }

    public void UpdateProgress(MissionData mission)
    {
        missionProgressText.text = $"{mission.currentProgress} / {mission.goal}";
        missionProgressBar.value = (float)mission.currentProgress / mission.goal;
    }
}
