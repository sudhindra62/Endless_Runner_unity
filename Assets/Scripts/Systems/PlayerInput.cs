
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerController.CurrentState == PlayerState.Dead) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerMovement.ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerMovement.ChangeLane(1);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerController.Jump();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerController.Slide();
        }
    }
}
