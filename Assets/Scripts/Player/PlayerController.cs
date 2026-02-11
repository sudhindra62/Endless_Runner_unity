using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 12f;

    [Header("Jump & Slide")]
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float slideDuration = 1f;

    [Header("References")]
    public Animator animator;
    public AudioSource audioSource;

    CharacterController controller;
    Vector3 velocity;

    int currentLane = 1;
    float originalHeight;
    Vector3 originalCenter;

    bool isSliding;
    bool isDead;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        Move();
    }

    public void Move()
    {
        Vector3 move = Vector3.forward * forwardSpeed;

        Vector3 targetPos = transform.position.z * Vector3.forward;
        if (currentLane == 0) targetPos += Vector3.left * laneDistance;
        if (currentLane == 2) targetPos += Vector3.right * laneDistance;

        float deltaX = targetPos.x - transform.position.x;
        move.x = deltaX * laneSwitchSpeed;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        controller.Move((move + velocity) * Time.deltaTime);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow)) Slide();
    }

    public void ChangeLane(int dir)
    {
        int target = currentLane + dir;
        if (target < 0 || target > 2) return;
        currentLane = target;
    }

    public void Jump()
    {
        if (!controller.isGrounded || isSliding) return;
        velocity.y = jumpForce;
        animator?.SetTrigger("Jump");
    }

    public void Slide()
    {
        if (!controller.isGrounded || isSliding) return;
        StartCoroutine(SlideRoutine());
    }

    System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;
        animator?.SetTrigger("Slide");

        controller.height = originalHeight / 2;
        controller.center = originalCenter / 2;

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalHeight;
        controller.center = originalCenter;
        isSliding = false;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator?.SetTrigger("Die");
        ScoreManager.instance.GameOver();
        Time.timeScale = 0f;
    }

    public void ResetPlayer()
    {
        isDead = false;
        velocity = Vector3.zero;
        currentLane = 1;

        controller.enabled = false;
        transform.position = Vector3.zero;
        controller.enabled = true;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
{
    if (hit.collider.CompareTag("Coin"))
    {
        Coin coin = hit.collider.GetComponent<Coin>();
        if (coin != null)
        {
            coin.Collect();
        }
    }
}

}
