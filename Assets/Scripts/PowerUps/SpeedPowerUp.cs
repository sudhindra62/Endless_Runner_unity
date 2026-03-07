
using UnityEngine;

[CreateAssetMenu(fileName = "New Speed PowerUp", menuName = "PowerUps/Speed")]
public class SpeedPowerUp : PowerUp
{
    public float speedMultiplier = 2f;

    public override void Activate(GameObject player)
    {
        base.Activate(player);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.movementSpeed *= speedMultiplier;
        }
    }

    public override void Deactivate(GameObject player)
    {
        base.Deactivate(player);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.movementSpeed /= speedMultiplier;
        }
    }
}
