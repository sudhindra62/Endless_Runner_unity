
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class MainMenuPanel : UIPanel
    {
        [Header("UI Elements")]
        [SerializeField] private Button playButton;

        public override void Setup(Managers.UIManager uiManager)
        {
            base.Setup(uiManager);
            playButton.onClick.AddListener(() => _uiManager.OnPlayButtonPressed?.Invoke());
        }
    }
}
