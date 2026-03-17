using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private float originalSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, speed);
        rb.velocity = movement;
    }

    public void ActivateSpeedBoost(float boost, float duration)
    {
        speed += boost;
        Invoke(nameof(DeactivateSpeedBoost), duration);
    }

    private void DeactivateSpeedBoost()
    {
        speed = originalSpeed;
    }
}
