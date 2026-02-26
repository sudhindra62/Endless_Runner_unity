using UnityEngine;

namespace EndlessRunner.UI.Bindings
{
    public class ReviveUIBinder : MonoBehaviour
    {
        public static ReviveUIBinder Instance { get; private set; }

        [SerializeField] private GameObject revivePopup;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
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
