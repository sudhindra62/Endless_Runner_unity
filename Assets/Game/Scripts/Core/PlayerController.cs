using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float laneWidth = 4f;

    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Change lanes
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }

        // Update lane position
        Vector3 targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneWidth;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }

    void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        if (newLane >= 0 && newLane <= 2)
        {
            currentLane = newLane;
        }
    }
}
