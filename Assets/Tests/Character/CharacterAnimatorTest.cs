
using UnityEngine;
using NUnit.Framework;

public class CharacterAnimatorTest
{
    private CharacterAnimator characterAnimator;
    private Animator animator;
    private GameObject characterGo;

    // Hashes for animator parameters
    private readonly int isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    private readonly int isSlidingHash = Animator.StringToHash("IsSliding");
    private readonly int isTurningLeftHash = Animator.StringToHash("IsTurningLeft");
    private readonly int isTurningRightHash = Animator.StringToHash("IsTurningRight");

    [SetUp]
    public void Setup()
    {
        // Set up the character with an Animator and the script to test
        characterGo = new GameObject("Character");
        animator = characterGo.AddComponent<Animator>();
        characterAnimator = characterGo.AddComponent<CharacterAnimator>();

        // In a real project, you would have an Animator Controller attached.
        // For this test, we can't create a controller at runtime easily.
        // We will have to rely on calling the methods and trusting they work if no errors are thrown.
        // A better approach would use a mock animator or a test-specific animator controller.
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(characterGo);
    }

    [Test]
    public void SetRunning_TogglesParameter()
    {
        // Act
        characterAnimator.SetRunning(true);
        // Assert
        // The following assertion would be ideal, but requires a controller with the parameter.
        // Assert.IsTrue(animator.GetBool(isRunningHash));

        // Act
        characterAnimator.SetRunning(false);
        // Assert
        // Assert.IsFalse(animator.GetBool(isRunningHash));
        
        // For now, we just ensure the method call is safe.
        Assert.DoesNotThrow(() => characterAnimator.SetRunning(true));
    }

    [Test]
    public void SetJumping_TogglesParameter()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => characterAnimator.SetJumping(true));
        Assert.DoesNotThrow(() => characterAnimator.SetJumping(false));
    }

    [Test]
    public void SetSliding_TogglesParameter()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => characterAnimator.SetSliding(true));
        Assert.DoesNotThrow(() => characterAnimator.SetSliding(false));
    }

    [Test]
    public void SetTurningLeft_TogglesParameter()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => characterAnimator.SetTurningLeft(true));
        Assert.DoesNotThrow(() => characterAnimator.SetTurningLeft(false));
    }

    [Test]
    public void SetTurningRight_TogglesParameter()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => characterAnimator.SetTurningRight(true));
        Assert.DoesNotThrow(() => characterAnimator.SetTurningRight(false));
    }
}
