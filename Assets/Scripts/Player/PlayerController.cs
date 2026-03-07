using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check for ground contact
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Sideways movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * moveHorizontal * moveSpeed * Time.deltaTime);

        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
