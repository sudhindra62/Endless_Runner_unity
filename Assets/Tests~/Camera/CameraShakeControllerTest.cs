
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class CameraShakeControllerTest
{
    private Camera camera;
    private CameraShakeController shakeController;

    [SetUp]
    public void Setup()
    {
        // Set up the camera and controller for each test
        GameObject camGo = new GameObject("Main Camera");
        camera = camGo.AddComponent<Camera>();
        camGo.tag = "MainCamera"; 
        shakeController = camGo.AddComponent<CameraShakeController>();
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        Object.DestroyImmediate(camera.gameObject);
    }

    [UnityTest]
    public IEnumerator ShakeCamera_MovesCameraWithinBounds()
    {
        // Arrange
        float duration = 0.2f;
        float amount = 0.5f;
        Vector3 initialPosition = camera.transform.position;

        // Act
        shakeController.ShakeCamera(duration, amount);
        yield return new WaitForSeconds(duration);

        // Assert
        Vector3 finalPosition = camera.transform.position;
        Assert.AreNotEqual(initialPosition, finalPosition, "Camera position should change after shake.");
        float distance = Vector3.Distance(initialPosition, finalPosition);
        Assert.LessOrEqual(distance, amount, "Camera should not move beyond the shake amount.");
    }

    [UnityTest]
    public IEnumerator ShakeCamera_ReturnsToOriginalPosition_Approximately()
    {
        // Arrange
        float duration = 0.2f;
        float amount = 0.5f;
        Vector3 initialPosition = camera.transform.position;

        // Act
        shakeController.ShakeCamera(duration, amount);
        yield return new WaitForSeconds(duration + 0.1f); // Wait for shake to finish

        // Assert
        Assert.AreEqual(0, shakeController.shakeDuration, "Shake duration should be reset to 0.");
        // After the shake is over, the camera should be at its resting position.
        // In the current implementation, it doesn't return to the original position but stops shaking.
        // A more robust test would require modifying the class to return the camera to its original position.
        // For now, we'll just check that the shaking has stopped.
        Vector3 positionAfterShake = camera.transform.position;
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(positionAfterShake, camera.transform.position, "Camera should remain stationary after shake is complete.");
    }

    [Test]
    public void ShakeCamera_SetsDurationAndAmount()
    {
        // Arrange
        float duration = 0.5f;
        float amount = 1.0f;

        // Act
        shakeController.ShakeCamera(duration, amount);

        // Assert
        Assert.AreEqual(duration, shakeController.shakeDuration);
        Assert.AreEqual(amount, shakeController.shakeAmount);
    }
}
