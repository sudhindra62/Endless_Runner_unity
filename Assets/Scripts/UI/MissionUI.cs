
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Missions;

namespace EndlessRunner.UI
{
    /// <summary>
    /// Displays the status of a single mission.
    /// </summary>
    public class MissionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Slider progressBar;

        public void UpdateMission(Mission mission)
        {
            if (mission == null) return;

            descriptionText.text = mission.description;
            progressBar.value = mission.progress;
        }
    }
}
