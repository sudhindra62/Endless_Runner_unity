using UnityEngine;

public class PerfectDodgeDetector : MonoBehaviour
{
    [Tooltip("The distance from an obstacle to be considered a perfect dodge.")]
    public float perfectDodgeDistance = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance <= perfectDodgeDistance)
            {
                StyleManager.Instance.PerfectDodge();
            }
        }
    }
}
