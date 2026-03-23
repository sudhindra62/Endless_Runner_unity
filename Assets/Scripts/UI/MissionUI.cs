
using UnityEngine;
using UnityEngine.UI;
using TMPro;


    /// <summary>
    /// Displays the status of a single mission.
    /// </summary>
    public class MissionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Slider progressBar;

        public void UpdateMission(MissionData mission)
        {
            if (mission == null) return;

            descriptionText.text = mission.missionDescription;
            progressBar.value = (float)mission.currentProgress / mission.goal;
        }
    }

