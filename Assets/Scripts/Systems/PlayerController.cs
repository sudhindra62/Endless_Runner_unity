
using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Running,
    Jumping,
    Sliding,
    Dead
}

[RequireComponent(typeof(CharacterController), typeof(AudioSource), typeof(PlayerMovement), typeof(PlayerCollisionHandler), typeof(PlayerPowerUpHandler), typeof(PlayerDeathHandler), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Slide")]
    [SerializeField] private float slideDuration = 1f;

    private PlayerState currentState;
    private float slideTimer;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    public Animator Animator => animator;
    public PlayerState CurrentState => currentState;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        SetState(PlayerState.Running);
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Jumping:
                if (playerMovement.IsGrounded())
                {
                    SetState(PlayerState.Running);
                }
                break;
            case PlayerState.Sliding:
                slideTimer -= Time.deltaTime;
                if(slideTimer <= 0)
                {
                    SetState(PlayerState.Running);
                }
                break;
        }
    }

    public void SetState(PlayerState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case PlayerState.Running:
                animator.Play("Run");
                ResetControllerSize();
                break;
            case PlayerState.Jumping:
                animator.Play("Jump");
                break;
            case PlayerState.Sliding:
                animator.Play("Slide");
                SetControllerSlideSize();
                slideTimer = slideDuration;
                break;
            case PlayerState.Dead:
                // Handled by PlayerDeathHandler
                break;
        }
    }

    public void Jump()
    {
        if (currentState == PlayerState.Running)
        {
            playerMovement.Jump();
            SetState(PlayerState.Jumping);
        }
    }

    public void Slide()
    {
        if (currentState == PlayerState.Running)
        {
            SetState(PlayerState.Sliding);
        }
    }

    public void ResetPlayerToStart()
    {
        playerMovement.ResetPosition();
        SetState(PlayerState.Running);
    }

    private void SetControllerSlideSize()
    {
        characterController.height = characterController.height / 2;
        characterController.center = characterController.center / 2;
    }

    private void ResetControllerSize()
    {
        characterController.height = characterController.height * 2;
        characterController.center = characterController.center * 2;
    }
}
