
using UnityEngine;

namespace EndlessRunner.UI
{
    public class LeaderboardUI : MonoBehaviour
    {
        public void ShowPanel() { gameObject.SetActive(true); }
        public void HidePanel() { gameObject.SetActive(false); }
    }
}
