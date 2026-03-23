
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class CharacterMotorTest
{
    private GameObject characterGo;
    private CharacterMotor motor;
    private CharacterController controller;

    [SetUp]
    public void Setup()
    {
        // Setup character with all required components
        characterGo = new GameObject("Character");
        controller = characterGo.AddComponent<CharacterController>();
        characterGo.AddComponent<CharacterAnimator>(); // Dependency
        motor = characterGo.AddComponent<CharacterMotor>();

        // Set initial position to avoid issues with other objects in the scene
        characterGo.transform.position = Vector3.zero;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(characterGo);
    }

    [UnityTest]
    public IEnumerator Move_CharacterMovesForward() 
    {
        // Arrange
        float initialZ = characterGo.transform.position.z;

        // Act
        yield return null; // Wait for one frame (Update)

        // Assert
        Assert.Greater(characterGo.transform.position.z, initialZ, "Character should move forward along the Z-axis.");
    }

    [UnityTest]
    public IEnumerator Jump_IncreasesHeight_WhenGrounded() 
    {
        // Arrange
        // Ensure the character is grounded
        yield return null;
        Assert.IsTrue(controller.isGrounded, "Character should be grounded at the start.");

        float initialY = characterGo.transform.position.y;

        // Act
        motor.Jump();
        yield return new WaitForSeconds(0.1f); // Allow some time to move upwards

        // Assert
        Assert.Greater(characterGo.transform.position.y, initialY, "Character's Y position should increase after jumping.");
    }

    [Test]
    public void ChangeLane_UpdatesTargetLane()
    {
        // Arrange
        int initialLane = motor.currentLane;

        // Act
        motor.ChangeLane(1); // Move right

        // Assert
        Assert.AreEqual(initialLane + 1, motor.currentLane, "currentLane should be incremented.");

        // Act
        motor.ChangeLane(-1); // Move left

        // Assert
        Assert.AreEqual(initialLane, motor.currentLane, "currentLane should be decremented.");
    }

    [Test]
    public void ChangeLane_DoesNotExceedLaneBounds()
    {
        // Arrange
        motor.currentLane = 1; // Start at the rightmost lane

        // Act
        motor.ChangeLane(1); // Try to move further right

        // Assert
        Assert.AreEqual(1, motor.currentLane, "Should not be able to move beyond the right lane.");

        // Arrange
        motor.currentLane = -1; // Start at the leftmost lane

        // Act
        motor.ChangeLane(-1); // Try to move further left

        // Assert
        Assert.AreEqual(-1, motor.currentLane, "Should not be able to move beyond the left lane.");
    }

    [UnityTest]
    public IEnumerator Slide_ReducesControllerHeight()
    {
        // Arrange
        float initialHeight = controller.height;

        // Act
        motor.Slide();
        yield return null;

        // Assert
        Assert.Less(controller.height, initialHeight, "Controller height should be reduced when sliding.");

        // Act
        yield return new WaitForSeconds(motor.slideDuration);

        // Assert
        Assert.AreEqual(initialHeight, controller.height, "Controller height should be restored after sliding.");
    }
}
