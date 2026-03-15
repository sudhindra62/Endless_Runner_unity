
using UnityEngine;

namespace EndlessRunner.UI
{
    public class DailyRewardUI : MonoBehaviour
    {
        public void ShowPanel() { gameObject.SetActive(true); }
        public void HidePanel() { gameObject.SetActive(false); }
    }
}
