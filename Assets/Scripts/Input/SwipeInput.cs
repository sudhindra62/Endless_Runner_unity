using UnityEngine;
using System;

/// <summary>
/// Unified SwipeInput
/// Preserves:
/// - Singleton
/// - Event-based architecture
/// - Keyboard support (Editor)
/// - Touch support (Mobile)
/// - DontDestroyOnLoad
/// </summary>
public class SwipeInput : MonoBehaviour
{
    public static SwipeInput Instance { get; private set; }

    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    [Header("Input Settings")]
    [SerializeField] private float minSwipeDistance = 50f;

    private Vector2 touchStartPos;
    private bool isSwiping;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            HandleKeyboardInput();
        }
        else
        {
            HandleTouchInput();
        }
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            OnSwipeUp?.Invoke();

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            OnSwipeDown?.Invoke();

        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            OnSwipeLeft?.Invoke();

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            OnSwipeRight?.Invoke();
    }

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

    private void ProcessSwipe(Vector2 touchEndPos)
    {
        Vector2 swipeDelta = touchEndPos - touchStartPos;

        if (swipeDelta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
                OnSwipeRight?.Invoke();
            else
                OnSwipeLeft?.Invoke();
        }
        else
        {
            if (swipeDelta.y > 0)
                OnSwipeUp?.Invoke();
            else
                OnSwipeDown?.Invoke();
        }
    }
}
