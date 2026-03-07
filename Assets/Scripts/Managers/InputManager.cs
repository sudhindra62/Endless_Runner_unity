
using System;
using UnityEngine;

public enum SwipeDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}

public class InputManager : Singleton<InputManager>
{
    // --- EVENTS ---
    public static event Action<SwipeDirection> OnSwipe;
    public static event Action OnJump; // Specific event for jumping (e.g., Swipe Up)
    public static event Action OnSlide; // Specific event for sliding (e.g., Swipe Down)
    public static event Action OnTap;

    // --- STATE ---
    private Vector2 _startTouchPosition;
    private bool _isInputEnabled = true;
    private TutorialAction _allowedAction = TutorialAction.None; // For tutorial control

    [Header("Input Settings")]
    [SerializeField] private float minSwipeDistance = 50f;

    // --- UNITY LIFECYCLE ---
    private void Update()
    {
        if (!_isInputEnabled) return;

        HandleKeyboardInput();
        HandleTouchInput();
    }

    // --- PUBLIC API ---

    /// <summary>
    /// Globally enables or disables all input processing.
    /// </summary>
    public void SetInputEnabled(bool isEnabled, TutorialAction allowedAction = TutorialAction.None)
    {
        _isInputEnabled = isEnabled;
        _allowedAction = allowedAction;
    }

    // --- PRIVATE METHODS ---

    private void HandleKeyboardInput()
    {
        // Keyboard input for editor testing
        if (IsActionAllowed(TutorialAction.SwipeLeft) && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
        {
            OnSwipe?.Invoke(SwipeDirection.Left);
        }
        if (IsActionAllowed(TutorialAction.SwipeRight) && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
        {
            OnSwipe?.Invoke(SwipeDirection.Right);
        }
        if (IsActionAllowed(TutorialAction.Jump) && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            OnSwipe?.Invoke(SwipeDirection.Up);
            OnJump?.Invoke();
        }
        if (IsActionAllowed(TutorialAction.Slide) && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            OnSwipe?.Invoke(SwipeDirection.Down);
            OnSlide?.Invoke();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    Vector2 endTouchPosition = touch.position;
                    Vector2 swipeDelta = endTouchPosition - _startTouchPosition;

                    if (swipeDelta.magnitude > minSwipeDistance)
                    {
                        DetectSwipe(swipeDelta);
                    }
                    else
                    {
                        OnTap?.Invoke();
                    }
                    break;
            }
        }
    }

    private void DetectSwipe(Vector2 swipeDelta)
    {
        SwipeDirection direction;
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            direction = swipeDelta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            direction = swipeDelta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }

        if (IsActionAllowed(direction))
        {
            OnSwipe?.Invoke(direction);

            // Trigger specific jump/slide events based on swipe direction
            if (direction == SwipeDirection.Up) OnJump?.Invoke();
            if (direction == SwipeDirection.Down) OnSlide?.Invoke();
        }
    }
    
    private bool IsActionAllowed(TutorialAction action)
    {
        if (_allowedAction == TutorialAction.None) return true;
        return action == _allowedAction;
    }

    private bool IsActionAllowed(SwipeDirection direction)
    {
        if (_allowedAction == TutorialAction.None) return true;

        return direction switch
        {
            SwipeDirection.Left => _allowedAction == TutorialAction.SwipeLeft,
            SwipeDirection.Right => _allowedAction == TutorialAction.SwipeRight,
            SwipeDirection.Up => _allowedAction == TutorialAction.Jump,
            SwipeDirection.Down => _allowedAction == TutorialAction.Slide,
            _ => false,
        };
    }
}
