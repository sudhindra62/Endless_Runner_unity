using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A centralized manager for handling all player input using the New Input System.
/// Supports Keyboard (PC) and Swipes (Mobile).
/// Global scope Singleton for project-wide accessibility.
/// </summary>
public enum SwipeDirection { None, Up, Down, Left, Right }

public class InputManager : Singleton<InputManager>
{
    // --- Events ---
    public event Action<SwipeDirection> OnSwipe;
    public event Action<int> OnLaneChange; // -1 for left, 1 for right
    public event Action OnJump;

    [Header("Swipe Configuration")]
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private float maxSwipeTime = 1f;

    private Vector2 _touchStartPos;
    private float _touchStartTime;

    private void Update()
    {
        HandleKeyboardInput();
        HandleTouchInput();
    }

    private void HandleKeyboardInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(SwipeDirection.Up);
            OnJump?.Invoke();
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(SwipeDirection.Down);
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(SwipeDirection.Left);
            OnLaneChange?.Invoke(-1);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            OnSwipe?.Invoke(SwipeDirection.Right);
            OnLaneChange?.Invoke(1);
        }
    }

    private void HandleTouchInput()
    {
        if (Touchscreen.current == null) return;

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            _touchStartPos = Touchscreen.current.primaryTouch.position.ReadValue();
            _touchStartTime = Time.time;
        }

        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
        {
            if (Time.time - _touchStartTime > maxSwipeTime) return;

            Vector2 touchEndPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 delta = touchEndPos - _touchStartPos;

            if (delta.magnitude < minSwipeDistance) return;

            DetectSwipe(delta);
        }
    }

    private void DetectSwipe(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // Horizontal Swipe
            if (delta.x > 0)
            {
                OnSwipe?.Invoke(SwipeDirection.Right);
                OnLaneChange?.Invoke(1);
            }
            else
            {
                OnSwipe?.Invoke(SwipeDirection.Left);
                OnLaneChange?.Invoke(-1);
            }
        }
        else
        {
            // Vertical Swipe
            if (delta.y > 0)
            {
                OnSwipe?.Invoke(SwipeDirection.Up);
                OnJump?.Invoke();
            }
            else
            {
                OnSwipe?.Invoke(SwipeDirection.Down);
            }
        }
    }
}
