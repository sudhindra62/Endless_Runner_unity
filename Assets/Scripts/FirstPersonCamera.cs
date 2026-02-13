
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1, 0.5f);

    void LateUpdate()
    {
        if (!target) return;

        transform.position = target.position + offset;
        transform.rotation = target.rotation;
    }
}
