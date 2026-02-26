
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerDeathHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCollisionHandler))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public bool IsDead { get; set; }

    private PlayerMovement playerMovement;
    private PlayerDeathHandler deathHandler;
    private CharacterController controller;
    
    private int dieTriggerHash;

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

        playerMovement = GetComponent<PlayerMovement>();
        deathHandler = GetComponent<PlayerDeathHandler>();
        controller = GetComponent<CharacterController>();
        
        dieTriggerHash = Animator.StringToHash("Die");
    }

    private void OnEnable()
    {
        ReviveManager.OnReviveSuccess += Revive;
    }

    private void OnDisable()
    {
        ReviveManager.OnReviveSuccess -= Revive;
    }

    private void Update()
    {
        if (IsDead) return;
        playerMovement.Move();
    }
    
    public void Revive()
    {
        IsDead = false;

        // The Die trigger should be reset to allow the animation state machine to return to normal.
        playerMovement.animator?.ResetTrigger(dieTriggerHash);
        playerMovement.animator?.Play("Run"); // Or your default running animation state
        
        playerMovement.ResetMovement();
    }

    public void ResetPlayer()
    {
        IsDead = false;
        playerMovement.ResetMovement();

        // Temporarily disable the controller to teleport the player
        controller.enabled = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        controller.enabled = true;
    }
}
