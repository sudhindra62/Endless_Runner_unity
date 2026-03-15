
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class PausePanel : UIPanel
    {
        [Header("UI Elements")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;

        public override void Setup(Managers.UIManager uiManager)
        {
            base.Setup(uiManager);
            resumeButton.onClick.AddListener(() => _uiManager.OnPauseButtonPressed?.Invoke()); // Toggles pause off
            mainMenuButton.onClick.AddListener(() => _uiManager.OnMainMenuButtonPressed?.Invoke());
        }
    }
}
