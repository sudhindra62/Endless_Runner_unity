
using UnityEngine;

/// <summary>
/// Manages the character's animations based on its movement state.
/// Created by Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    // Animation hashes for performance
    private readonly int isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    private readonly int isSlidingHash = Animator.StringToHash("IsSliding");
    private readonly int isTurningLeftHash = Animator.StringToHash("IsTurningLeft");
    private readonly int isTurningRightHash = Animator.StringToHash("IsTurningRight");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool(isRunningHash, isRunning);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool(isJumpingHash, isJumping);
    }

    public void SetSliding(bool isSliding)
    {
        animator.SetBool(isSlidingHash, isSliding);
    }

    public void SetTurningLeft(bool isTurningLeft)
    {
        animator.SetBool(isTurningLeftHash, isTurningLeft);
    }

    public void SetTurningRight(bool isTurningRight)
    {
        animator.SetBool(isTurningRightHash, isTurningRight);
    }
}
