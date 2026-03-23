
using UnityEngine;
using TMPro;

    public class UIPanel_Tutorial : UIPanel
    {
        public override UIPanelType PanelType => UIPanelType.Tutorial;

        [SerializeField] private TextMeshProUGUI instructionText;

        public void Show(string message, float duration)
        {
            instructionText.text = message;
            base.Show();
        }
    }
