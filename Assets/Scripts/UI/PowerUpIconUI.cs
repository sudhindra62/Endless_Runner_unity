
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public class PowerUpIconUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI timerText;

        private PowerUpDefinition powerUpDefinition;
        private float timer;

        public void SetPowerUp(PowerUpDefinition definition)
        {
            powerUpDefinition = definition;
            iconImage.sprite = definition.icon;
            timer = definition.duration;
            gameObject.SetActive(true);
        }

        public void UpdateTimer(float deltaTime)
        {
            if (powerUpDefinition == null) return;

            timer -= deltaTime;
            timerText.text = Mathf.CeilToInt(timer).ToString();

            if (timer <= 0)
            {
                gameObject.SetActive(false);
                powerUpDefinition = null;
            }
        }

        public bool IsActive() => powerUpDefinition != null;
    }
