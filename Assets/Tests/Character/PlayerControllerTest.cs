
using UnityEngine;
using NUnit.Framework;

public class PlayerControllerTest
{
    private GameObject characterGo;
    private PlayerController playerController;
    private MockCharacterMotor mockMotor;

    // Mock CharacterMotor to intercept calls
    public class MockCharacterMotor : CharacterMotor
    {
        public bool jumpCalled = false;
        public bool slideCalled = false;
        public int laneChangeDirection = 0;

        public override void Jump()
        {
            jumpCalled = true;
        }

        public override void Slide()
        {
            slideCalled = true;
        }

        public override void ChangeLane(int direction)
        {
            laneChangeDirection = direction;
        }
    }

    [SetUp]
    public void Setup()
    {
        // Setup character with PlayerController and Mock motor
        characterGo = new GameObject("Character");
        playerController = characterGo.AddComponent<PlayerController>();
        mockMotor = characterGo.AddComponent<MockCharacterMotor>();
        characterGo.AddComponent<CharacterController>(); // Dependency for motor
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(characterGo);
    }

    // Keyboard input tests require a way to simulate key presses, which is complex in standard Unity tests.
    // We will skip testing keyboard input directly and focus on the swipe logic.
    // In a real project, one might use a package like Unity's InputSystem for easier testable input.

    [Test]
    public void HandleSwipe_CallsJumpOnUpSwipe()
    {
        // Arrange
        Vector2 swipeDelta = new Vector2(0, 100); // Upward swipe

        // Act
        playerController.HandleSwipe(swipeDelta);

        // Assert
        Assert.IsTrue(mockMotor.jumpCalled, "Jump should be called on an upward swipe.");
    }

    [Test]
    public void HandleSwipe_CallsSlideOnDownSwipe()
    {
        // Arrange
        Vector2 swipeDelta = new Vector2(0, -100); // Downward swipe

        // Act
        playerController.HandleSwipe(swipeDelta);

        // Assert
        Assert.IsTrue(mockMotor.slideCalled, "Slide should be called on a downward swipe.");
    }

    [Test]
    public void HandleSwipe_CallsChangeLaneOnRightSwipe()
    {
        // Arrange
        Vector2 swipeDelta = new Vector2(100, 0); // Rightward swipe

        // Act
        playerController.HandleSwipe(swipeDelta);

        // Assert
        Assert.AreEqual(1, mockMotor.laneChangeDirection, "ChangeLane(1) should be called on a rightward swipe.");
    }

    [Test]
    public void HandleSwipe_CallsChangeLaneOnLeftSwipe()
    {
        // Arrange
        Vector2 swipeDelta = new Vector2(-100, 0); // Leftward swipe

        // Act
        playerController.HandleSwipe(swipeDelta);

        // Assert
        Assert.AreEqual(-1, mockMotor.laneChangeDirection, "ChangeLane(-1) should be called on a leftward swipe.");
    }
    
    [Test]
    public void HandleSwipe_IgnoresDiagonalSwipes()
    {
        // Arrange: A swipe that is more horizontal than vertical
        Vector2 horizontalSwipe = new Vector2(100, 80);
        // Arrange: A swipe that is more vertical than horizontal
        Vector2 verticalSwipe = new Vector2(80, 100);

        // Act & Assert for horizontal
        playerController.HandleSwipe(horizontalSwipe);
        Assert.AreEqual(1, mockMotor.laneChangeDirection, "Should register a horizontal swipe.");
        Assert.IsFalse(mockMotor.jumpCalled, "Should not jump on a mostly horizontal swipe.");
        Assert.IsFalse(mockMotor.slideCalled, "Should not slide on a mostly horizontal swipe.");

        // Reset mock
        mockMotor.laneChangeDirection = 0;

        // Act & Assert for vertical
        playerController.HandleSwipe(verticalSwipe);
        Assert.IsTrue(mockMotor.jumpCalled, "Should register a vertical swipe (jump).");
        Assert.AreEqual(0, mockMotor.laneChangeDirection, "Should not change lanes on a mostly vertical swipe.");
    }
}
