
using UnityEngine;

[CreateAssetMenu(fileName = "New Jump PowerUp", menuName = "PowerUps/Jump")]
public class JumpPowerUp : PowerUp
{
    public float jumpForceMultiplier = 2f;

    public override void Activate(GameObject player)
    {
        base.Activate(player);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.jumpForce *= jumpForceMultiplier;
        }
    }

    public override void Deactivate(GameObject player)
    {
        base.Deactivate(player);
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.jumpForce /= jumpForceMultiplier;
        }
    }
}
