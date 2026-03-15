
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private Text coinText;
        [SerializeField] private Image[] uiAccents;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void UpdateCoinCount(int count)
        {
            if (coinText != null)
            {
                coinText.text = "Coins: " + count;
            }
        }

        public void SetAccentColor(Color color)
        {
            foreach (Image accent in uiAccents)
            {
                if (accent != null)
                {
                    accent.color = color;
                }
            }
        }
    }
}
