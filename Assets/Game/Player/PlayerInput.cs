
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if (SwipeInput.Instance != null)
        {
            SwipeInput.Instance.OnSwipeLeft += HandleSwipeLeft;
            SwipeInput.Instance.OnSwipeRight += HandleSwipeRight;
            SwipeInput.Instance.OnSwipeUp += HandleSwipeUp;
            SwipeInput.Instance.OnSwipeDown += HandleSwipeDown;
        }
    }

    private void OnDisable()
    {
        if (SwipeInput.Instance != null)
        {
            SwipeInput.Instance.OnSwipeLeft -= HandleSwipeLeft;
            SwipeInput.Instance.OnSwipeRight -= HandleSwipeRight;
            SwipeInput.Instance.OnSwipeUp -= HandleSwipeUp;
            SwipeInput.Instance.OnSwipeDown -= HandleSwipeDown;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) HandleSwipeLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow)) HandleSwipeRight();
        if (Input.GetKeyDown(KeyCode.UpArrow)) HandleSwipeUp();
        if (Input.GetKeyDown(KeyCode.DownArrow)) HandleSwipeDown();
    }

    private void HandleSwipeLeft()
    {
        playerMovement.ChangeLane(-1);
    }

    private void HandleSwipeRight()
    {
        playerMovement.ChangeLane(1);
    }

    private void HandleSwipeUp()
    {
        playerMovement.Jump();
    }

    private void HandleSwipeDown()
    {
        playerMovement.Slide();
    }
}
