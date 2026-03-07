
using System;
using UnityEngine;

/// <summary>
/// Handles all player input and broadcasts it as events.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a decoupled, event-driven architecture.
/// </summary>
public class InputManager : Singleton<InputManager>
{
    // --- EVENTS ---
    public static event Action OnSwipeLeft;
    public static event Action OnSwipeRight;
    public static event Action OnSwipeUp;
    public static event Action OnSwipeDown;
    public static event Action OnTap;

    // --- PRIVATE STATE ---
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    [Header("Input Settings")]
    [SerializeField] private float minSwipeDistance = 50f;

    // --- UNITY LIFECYCLE ---
    private void Update()
    {
        // --- Keyboard Input (for testing in editor) ---
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            OnSwipeLeft?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            OnSwipeRight?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            OnSwipeUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            OnSwipeDown?.Invoke();
        }

        // --- Touch Input ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (!isSwiping) return;
                    endTouchPosition = touch.position;
                    DetectSwipe();
                    break;

                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        isSwiping = false;
                        // This could be interpreted as a tap if the swipe distance was not met
                        if (Vector2.Distance(startTouchPosition, endTouchPosition) < minSwipeDistance)
                        {
                            OnTap?.Invoke();
                        }
                    }
                    break;
            }
        }
    }

    // --- PRIVATE METHODS ---
    private void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (Mathf.Abs(swipeDelta.x) > minSwipeDistance || Mathf.Abs(swipeDelta.y) > minSwipeDistance)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // Horizontal Swipe
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
                // Vertical Swipe
                if (swipeDelta.y > 0)
                {
                    OnSwipeUp?.Invoke();
                }
                else
                {
                    OnSwipeDown?.Invoke();
                }
            }
            isSwiping = false; // End the swipe after one is detected
        }
    }
}
