
using UnityEngine;
using EndlessRunner.UI;

namespace EndlessRunner.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public HomeScreenUI homeScreen;
        public GameScreenUI gameScreen; // Assume this exists
        public PauseScreenUI pauseScreen; // Assume this exists

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowHomeScreen()
        {
            if(homeScreen) homeScreen.gameObject.SetActive(true);
            if(gameScreen) gameScreen.gameObject.SetActive(false);
            if(pauseScreen) pauseScreen.gameObject.SetActive(false);
        }

        public void ShowGameScreen()
        {
            if(homeScreen) homeScreen.gameObject.SetActive(false);
            if(gameScreen) gameScreen.gameObject.SetActive(true);
            if(pauseScreen) pauseScreen.gameObject.SetActive(false);
        }

        public void ShowPauseScreen()
        {
            if(pauseScreen) pauseScreen.gameObject.SetActive(true);
        }

        public void HidePauseScreen()
        {
            if(pauseScreen) pauseScreen.gameObject.SetActive(false);
        }

        public void UpdateCurrency(int coins, int gems)
        {
            if(homeScreen) homeScreen.UpdateCurrencyDisplay(coins, gems);
        }
    }
}
