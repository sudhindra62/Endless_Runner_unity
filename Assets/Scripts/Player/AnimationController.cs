
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Slide()
    {
        animator.SetTrigger("Slide");
    }

    public void Run()
    {
        animator.SetTrigger("Run");
    }
}
