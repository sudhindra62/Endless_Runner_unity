
using UnityEngine;

/// <summary>
/// Handles all character animations, listening to events from the CharacterMotor.
/// This keeps animation logic separate from movement logic.
/// </summary>
[RequireComponent(typeof(Animator), typeof(CharacterMotor))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterMotor motor;

    void Awake()
    {
        animator = GetComponent<Animator>();
        motor = GetComponent<CharacterMotor>();
    }

    private void OnEnable()
    {
        motor.OnJump += HandleJump;
        motor.OnSlideStart += HandleSlideStart;
        motor.OnSlideEnd += HandleSlideEnd;
        motor.OnLaneChange += HandleLaneChange;
    }

    private void OnDisable()
    {
        motor.OnJump -= HandleJump;
        motor.OnSlideStart -= HandleSlideStart;
        motor.OnSlideEnd -= HandleSlideEnd;
        motor.OnLaneChange -= HandleLaneChange;
    }

    void Start()
    {
        animator.SetBool("IsRunning", true);
    }

    private void HandleJump()
    {
        animator.SetTrigger("Jump");
    }

    private void HandleSlideStart()
    {
        animator.SetBool("IsSliding", true);
    }

    private void HandleSlideEnd()
    {
        animator.SetBool("IsSliding", false);
    }

    private void HandleLaneChange(int direction)
    {
        if (direction > 0)
        {
            animator.SetTrigger("TurnRight");
        }
        else
        {
            animator.SetTrigger("TurnLeft");
        }
    }
}
