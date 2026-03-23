
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class CameraControllerTest
{
    private CameraController cameraController;
    private Transform target;
    private GameObject cameraGo;

    [SetUp]
    public void Setup()
    {
        // Setup camera with controller
        cameraGo = new GameObject("Main Camera");
        cameraGo.AddComponent<Camera>();
        cameraGo.tag = "MainCamera"; // Ensure camera is discoverable
        cameraController = cameraGo.AddComponent<CameraController>();

        // Setup target
        GameObject targetGo = new GameObject("Player");
        targetGo.tag = "Player";
        target = targetGo.transform;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(cameraGo);
        Object.DestroyImmediate(target.gameObject);
    }

    [UnityTest]
    public IEnumerator Follow_MovesCameraTowardsTarget() 
    {
        // Arrange
        Vector3 initialCamPos = cameraGo.transform.position;
        target.position = new Vector3(10, 0, 10);

        // Act
        yield return null; // Wait for one frame for LateUpdate to run

        // Assert
        Vector3 newCamPos = cameraGo.transform.position;
        Assert.AreNotEqual(initialCamPos, newCamPos, "Camera should move from its initial position.");

        // Calculate expected position (without smoothing) to check direction
        Vector3 expectedPosition = target.position + cameraController.offset;
        float initialDistance = Vector3.Distance(initialCamPos, expectedPosition);
        float newDistance = Vector3.Distance(newCamPos, expectedPosition);

        Assert.Less(newDistance, initialDistance, "Camera should move closer to the target's offset position.");
    }

    [UnityTest]
    public IEnumerator LookAt_CameraOrientsTowardsTarget()
    {
        // Arrange
        target.position = new Vector3(5, 2, 5);
        yield return null;

        // Act
        Quaternion initialRotation = cameraGo.transform.rotation;
        target.position = new Vector3(-5, 2, -5); // Move the target
        yield return null; // Wait for LateUpdate

        // Assert
        Quaternion newRotation = cameraGo.transform.rotation;
        Assert.AreNotEqual(initialRotation, newRotation, "Camera should rotate to look at the new target position.");

        // Check if the camera is actually looking at the target
        Ray ray = new Ray(cameraGo.transform.position, cameraGo.transform.forward);
        Plane targetPlane = new Plane(Vector3.up, target.position);
        float enter;
        Assert.IsTrue(targetPlane.Raycast(ray, out enter), "Camera's forward vector should intersect the plane where the target lies.");
    }

    [Test]
    public void ShakeCamera_CallsShakeOnShakeController()
    {
        // Arrange
        var shakeController = cameraGo.GetComponent<CameraShakeController>();
        // This is a simple mock-like test. We can't easily check if a method was called without a mocking framework.
        // So, we'll just call the method and ensure no errors are thrown.
        // A better test would be to create a mock CameraShakeController.
        float duration = 0.5f;
        float amount = 0.8f;

        // Act & Assert
        Assert.DoesNotThrow(() => cameraController.ShakeCamera(duration, amount));
        // We can also check if the values were passed correctly if we make the fields public in CameraShakeController
        // For example: Assert.AreEqual(duration, shakeController.shakeDuration);
    }

}
