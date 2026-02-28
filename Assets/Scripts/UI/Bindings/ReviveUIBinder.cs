
using UnityEngine;

namespace EndlessRunner.UI.Bindings
{
    public class ReviveUIBinder : Singleton<ReviveUIBinder>
    {
        [SerializeField] private GameObject revivePopup;

        protected override void Awake()
        {
            base.Awake();
            revivePopup.SetActive(false);
        }

        public void Show()
        {
            revivePopup.SetActive(true);
        }

        public void Hide()
        {
            revivePopup.SetActive(false);
        }

        public void GemRevivePressed()
        {
            ReviveManager.Instance?.ReviveWithGems();
            Hide();
        }

        public void AdRevivePressed()
        {
            ReviveManager.Instance?.RevivePlayer();
            Hide();
        }

        public void TokenRevivePressed()
        {
            ReviveManager.Instance?.ReviveWithToken();
            Hide();
        }

        public void DeclinePressed()
        {
            Hide();
            GameFlowController.Instance?.EndRunFinal();
        }
    }
}
