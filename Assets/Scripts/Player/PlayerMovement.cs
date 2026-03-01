using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float baseGravity = -19.62f; // Doubled for a weightier feel
    [SerializeField] private float baseJumpForce = 8f;

    [Header("Caps")]
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float minSpeed = 5f;

    [Header("Components")]
    [SerializeField] private CharacterController controller;

    // Multiplier Dictionaries
    private readonly Dictionary<string, float> speedMultipliers = new Dictionary<string, float>();
    private readonly Dictionary<string, float> gravityMultipliers = new Dictionary<string, float>();
    private readonly Dictionary<string, float> jumpForceMultipliers = new Dictionary<string, float>();

    // State Dictionaries (to handle multiple sources)
    private readonly Dictionary<string, bool> jumpDisabledSources = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> reverseInputSources = new Dictionary<string, bool>();

    private Vector3 playerVelocity;
    private bool isGrounded;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerMovement>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Keep the player grounded
        }

        // Calculate current stats
        float currentSpeed = CalculateValue(baseSpeed, speedMultipliers, minSpeed, maxSpeed);
        float currentJumpForce = CalculateValue(baseJumpForce, jumpForceMultipliers);
        bool isJumpDisabled = jumpDisabledSources.Any(s => s.Value);
        bool isInputReversed = reverseInputSources.Any(s => s.Value);

        // --- Movement --- //
        float horizontalInput = Input.GetAxis("Horizontal") * (isInputReversed ? -1 : 1);
        Vector3 moveDirection = transform.forward * currentSpeed + transform.right * horizontalInput;
        controller.Move(moveDirection * Time.deltaTime);

        // --- Jumping --- //
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumpDisabled)
        {
            playerVelocity.y = currentJumpForce;
        }

        // --- Gravity --- //
        float currentGravity = CalculateValue(baseGravity, gravityMultipliers);
        playerVelocity.y += currentGravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private float CalculateValue(float baseValue, Dictionary<string, float> multipliers, float min = 0, float max = float.MaxValue)
    {
        float finalValue = baseValue;
        foreach (var multiplier in multipliers.Values)
        {
            finalValue *= multiplier;
        }
        return Mathf.Clamp(finalValue, min, max);
    }

    // --- Public Methods for Modifiers --- //

    public void ApplySpeedMultiplier(string sourceId, float multiplier) => speedMultipliers[sourceId] = multiplier;
    public void RemoveSpeedMultiplier(string sourceId) => speedMultipliers.Remove(sourceId);

    public void ApplyGravityMultiplier(string sourceId, float multiplier) => gravityMultipliers[sourceId] = multiplier;
    public void RemoveGravityMultiplier(string sourceId) => gravityMultipliers.Remove(sourceId);

    public void SetJumpDisabled(string sourceId, bool isDisabled) => jumpDisabledSources[sourceId] = isDisabled;
    public void RemoveJumpDisabled(string sourceId) => jumpDisabledSources.Remove(sourceId);

    public void SetReverseInput(string sourceId, bool isReversed) => reverseInputSources[sourceId] = isReversed;
    public void RemoveReverseInput(string sourceId) => reverseInputSources.Remove(sourceId);

    public void ResetState()
    {
        speedMultipliers.Clear();
        gravityMultipliers.Clear();
        jumpForceMultipliers.Clear();
        jumpDisabledSources.Clear();
        reverseInputSources.Clear();
    }
}
