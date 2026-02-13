using UnityEngine;
using System;

/// <summary>
/// A Singleton input manager that detects and broadcasts swipe gestures as events.
/// This provides a centralized and decoupled input system that other scripts can subscribe to.
/// It also includes keyboard support for use within the Unity Editor.
/// </summary>
public class SwipeInput : MonoBehaviour
{
    public static SwipeInput Instance { get; private set; }

    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    [Header("Input Settings")]
    [SerializeField] private float minSwipeDistance = 50f; // Minimum distance for a swipe to be registered.

    private Vector2 touchStartPos;
    private bool isSwiping;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene loads.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Choose the input method based on the platform.
        if (Application.isEditor)
        {
            HandleKeyboardInput();
        }
        else
        {
            HandleTouchInput();
        }
    }

    /// <summary>
    /// Handles keyboard input for testing in the Unity Editor.
    /// </summary>
    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnSwipeUp?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnSwipeDown?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnSwipeLeft?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnSwipeRight?.Invoke();
        }
    }

    /// <summary>
    /// Handles touch input for mobile devices.
    /// </summary>
    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                isSwiping = true;
                touchStartPos = touch.position;
                break;

            case TouchPhase.Ended:
                if (isSwiping)
                {
                    ProcessSwipe(touch.position);
                    isSwiping = false;
                }
                break;

            case TouchPhase.Canceled:
                isSwiping = false;
                break;
        }
    }

    /// <summary>
    /// Calculates the swipe direction and invokes the appropriate event.
    /// </summary>
    private void ProcessSwipe(Vector2 touchEndPos)
    {
        Vector2 swipeDelta = touchEndPos - touchStartPos;

        // Ensure the swipe was long enough to be intentional.
        if (swipeDelta.magnitude < minSwipeDistance) return;

        // Determine the primary axis of the swipe.
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            // Horizontal swipe
            if (swipeDelta.x > 0)
            {
                OnSwipeRight?.Invoke();
            }
            else
            {
                OnSwipeLeft?.Invoke();
            }
        }
        else
        { 
            // Vertical swipe
            if (swipeDelta.y > 0)
            {
                OnSwipeUp?.Invoke();
            }
            else
            {
                OnSwipeDown?.Invoke();
            }
        }
    }
}