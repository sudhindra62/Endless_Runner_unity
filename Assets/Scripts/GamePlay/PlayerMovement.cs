using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 8f;

    [Header("Lane Movement")]
    public float laneDistance = 3f;   // distance between lanes
    public float laneChangeSpeed = 10f;

    private int currentLane = 0; // -1 = left, 0 = center, 1 = right
    private bool isMoving = true;

    private Vector3 targetPosition;

    private void Start()
    {
          Time.timeScale = 1f;
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isMoving) return;

        HandleInput();
        MoveForward();
        MoveSideways();
    }

    /* -------------------------
     * Forward movement
     * ------------------------- */
   void MoveForward()
{
    Vector3 movement = Vector3.forward * forwardSpeed * Time.deltaTime;

    // Use CharacterController if exists
    CharacterController controller = GetComponent<CharacterController>();
    if (controller != null)
    {
        controller.Move(movement);
    }
    else
    {
        transform.Translate(movement, Space.World);
    }
}

    /* -------------------------
     * Lane input
     * ------------------------- */
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);
    }

    void ChangeLane(int direction)
    {
        int newLane = Mathf.Clamp(currentLane + direction, -1, 1);
        if (newLane == currentLane) return;

        currentLane = newLane;

        targetPosition = new Vector3(
            currentLane * laneDistance,
            transform.position.y,
            transform.position.z
        );
    }

    /* -------------------------
     * Smooth sideways movement
     * ------------------------- */
    void MoveSideways()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetPosition.x, Time.deltaTime * laneChangeSpeed);
        transform.position = pos;
    }

    /* -------------------------
     * Pause / Resume (Revive safe)
     * ------------------------- */
    public void Stop()
    {
        isMoving = false;
    }

    public void Resume()
    {
        isMoving = true;
    }
}
