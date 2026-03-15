
using UnityEngine;
using TMPro;

namespace EndlessRunner.UI
{
    public class UIPanel_Tutorial : UIPanel
    {
        [SerializeField] private TextMeshProUGUI instructionText;

        public void Show(string message, float duration)
        {
            instructionText.text = message;
            base.Show();
        }
    }
}
