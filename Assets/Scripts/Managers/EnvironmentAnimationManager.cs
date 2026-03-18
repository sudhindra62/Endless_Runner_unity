
using UnityEngine;

public class EnvironmentAnimationManager : MonoBehaviour
{
    public static EnvironmentAnimationManager Instance { get; private set; }

    [Header("Animatable Environment Elements")]
    [SerializeField] private Animator[] treeAnimators;
    [SerializeField] private Animator[] waterAnimators;
    [SerializeField] private Animator[] lightAnimators;
    [SerializeField] private Animator[] floatingObjectAnimators;
    [SerializeField] private Animator[] cityMovementAnimators;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AnimateTrees()
    {
        foreach (var animator in treeAnimators)
        {
            if(animator.gameObject.activeInHierarchy) animator.SetTrigger("startAnimation");
        }
    }

    public void AnimateWater()
    {
        foreach (var animator in waterAnimators)
        {
            if(animator.gameObject.activeInHierarchy) animator.SetTrigger("startAnimation");
        }
    }

    public void AnimateLights()
    {
        foreach (var animator in lightAnimators)
        {
            if(animator.gameObject.activeInHierarchy) animator.SetTrigger("startAnimation");
        }
    }

    public void AnimateFloatingObjects()
    {
        foreach (var animator in floatingObjectAnimators)
        {
            if(animator.gameObject.activeInHierarchy) animator.SetTrigger("startAnimation");
        }
    }

    public void AnimateCityMovement()
    {
        foreach (var animator in cityMovementAnimators)
        {
            if(animator.gameObject.activeInHierarchy) animator.SetTrigger("startAnimation");
        }
    }
}
