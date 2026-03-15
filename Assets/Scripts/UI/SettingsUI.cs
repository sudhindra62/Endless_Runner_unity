
using UnityEngine;

namespace EndlessRunner.UI
{
    public class SettingsUI : MonoBehaviour
    {
        public void ShowPanel() { gameObject.SetActive(true); }
        public void HidePanel() { gameObject.SetActive(false); }
    }
}
