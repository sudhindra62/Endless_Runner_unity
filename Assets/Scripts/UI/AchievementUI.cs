
using UnityEngine;

namespace EndlessRunner.UI
{
    public class AchievementUI : MonoBehaviour
    {
        public void ShowPanel() { gameObject.SetActive(true); }
        public void HidePanel() { gameObject.SetActive(false); }
    }
}
