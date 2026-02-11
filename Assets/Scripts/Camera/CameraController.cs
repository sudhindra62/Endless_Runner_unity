using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 5f;

    Vector3 shakeOffset;

    void LateUpdate()
    {
        Vector3 desired = target.position + offset + shakeOffset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(ShakeRoutine(intensity, duration));
    }

    System.Collections.IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            shakeOffset = Random.insideUnitSphere * intensity;
            t += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }
}
