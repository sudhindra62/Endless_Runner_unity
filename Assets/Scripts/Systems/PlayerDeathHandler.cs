
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerDeathHandler : MonoBehaviour
{
    private PlayerController playerController;
    private GameFlowController gameFlowController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        gameFlowController = FindFirstObjectByType<GameFlowController>();
    }

    public void Die()
    {
        if (playerController.CurrentState == PlayerState.Dead) return;

        playerController.SetState(PlayerState.Dead);
        playerController.Animator.Play("Die");
        gameFlowController.EndRun();
    }

    public void Revive()
    {
        playerController.SetState(PlayerState.Running);
        playerController.Animator.Play("Run");
        playerController.ResetPlayerToStart();
    }
}
