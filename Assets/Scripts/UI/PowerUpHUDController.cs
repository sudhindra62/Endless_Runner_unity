
using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Core;

namespace EndlessRunner.UI
{
    public class PowerUpHUDController : MonoBehaviour
    {
        [SerializeField] private GameObject powerUpIconPrefab;
        [SerializeField] private Transform iconContainer;

        private Dictionary<PowerUpDefinition, PowerUpIconUI> activeIcons = new Dictionary<PowerUpDefinition, PowerUpIconUI>();

        private void OnEnable()
        {
            GameEvents.OnPowerUpActivated += HandlePowerUpActivated;
            GameEvents.OnPowerUpDeactivated += HandlePowerUpDeactivated;
        }

        private void OnDisable()
        {
            GameEvents.OnPowerUpActivated -= HandlePowerUpActivated;
            GameEvents.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
        }

        private void Update()
        {
            foreach (var icon in activeIcons.Values)
            {
                icon.UpdateTimer(Time.deltaTime);
            }
        }

        private void HandlePowerUpActivated(PowerUpDefinition powerUp)
        {
            if (activeIcons.ContainsKey(powerUp))
            {
                activeIcons[powerUp].SetPowerUp(powerUp); // Reset timer
            }
            else
            {
                GameObject iconGO = Instantiate(powerUpIconPrefab, iconContainer);
                PowerUpIconUI iconUI = iconGO.GetComponent<PowerUpIconUI>();
                iconUI.SetPowerUp(powerUp);
                activeIcons.Add(powerUp, iconUI);
            }
        }

        private void HandlePowerUpDeactivated(PowerUpDefinition powerUp)
        {
            if (activeIcons.ContainsKey(powerUp))
            {
                Destroy(activeIcons[powerUp].gameObject);
                activeIcons.Remove(powerUp);
            }
        }
    }
}
