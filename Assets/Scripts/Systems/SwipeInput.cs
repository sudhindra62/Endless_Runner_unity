
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeInput : MonoBehaviour
{
    public delegate void Swipe(Vector2 direction);
    public event Swipe OnSwipe;

    [SerializeField] private float minSwipeDistance = 20f;
    [SerializeField] private float maxSwipeTime = 1f;
    [SerializeField] private float inputBufferTime = 0.2f;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private PlayerController playerController;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 lastSwipeDirection;
    private float lastSwipeTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
        ServiceLocator.Register<SwipeInput>(this);
    }

    private void Start()
    {
        playerController = ServiceLocator.Get<PlayerController>();
        if (playerController != null)
        {
            OnSwipe += playerController.OnSwipe;
        }
    }

    private void OnEnable()
    {
        touchPressAction.started += OnTouchStart;
        touchPressAction.canceled += OnTouchEnd;
    }

    private void OnDisable()
    {
        touchPressAction.started -= OnTouchStart;
        touchPressAction.canceled -= OnTouchEnd;
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        startPosition = touchPositionAction.ReadValue<Vector2>();
        startTime = Time.time;
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        float swipeTime = Time.time - startTime;
        Vector2 endPosition = touchPositionAction.ReadValue<Vector2>();
        float swipeDistance = Vector2.Distance(startPosition, endPosition);

        if (swipeTime < maxSwipeTime && swipeDistance > minSwipeDistance)
        {
            Vector2 direction = endPosition - startPosition;
            Vector2 swipeDirection = GetSwipeDirection(direction);

            if (Time.time - lastSwipeTime > inputBufferTime || swipeDirection != lastSwipeDirection)
            {
                OnSwipe?.Invoke(swipeDirection);
                lastSwipeDirection = swipeDirection;
                lastSwipeTime = Time.time;
            }
        }
    }

    private Vector2 GetSwipeDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    // Keyboard fallback for editor testing
    private void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(Vector2.left);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(Vector2.right);
        }
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(Vector2.up);
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(Vector2.down);
        }
    }
}
