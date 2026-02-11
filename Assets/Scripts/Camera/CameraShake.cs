using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public float duration = 0.2f;
    public float strength = 0.2f;

    Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float time = 0f;
        while (time < duration)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * strength;
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
