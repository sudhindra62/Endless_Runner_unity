using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void HandleDeath()
    {
        if (playerController != null)
        {
            playerController.Die();
        }
    }
}
