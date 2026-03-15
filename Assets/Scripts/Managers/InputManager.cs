
using System;
using UnityEngine;
using EndlessRunner.Core;
using UnityEngine.InputSystem;

namespace EndlessRunner.Managers
{
    public enum SwipeDirection
    {
        None, Up, Down, Left, Right
    }

    public class InputManager : Singleton<InputManager>
    {
        public event Action<SwipeDirection> OnSwipe;

        [Header("Swipe Configuration")]
        [SerializeField] private float minSwipeDistance = 50f;
        [SerializeField] private float maxSwipeTime = 1f;

        private Vector2 _touchStartPos;
        private float _touchStartTime;

        private void Update()
        {
            // --- Keyboard Input for Editor/PC ---
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                OnSwipe?.Invoke(SwipeDirection.Up);
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                OnSwipe?.Invoke(SwipeDirection.Down);
            }
            else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                OnSwipe?.Invoke(SwipeDirection.Left);
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                OnSwipe?.Invoke(SwipeDirection.Right);
            }

            // --- Touch Input for Mobile ---
            if (Touchscreen.current == null) return; // No touchscreen present

            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                _touchStartPos = Touchscreen.current.primaryTouch.position.ReadValue();
                _touchStartTime = Time.time;
            }

            if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            {
                if (Time.time - _touchStartTime > maxSwipeTime) return; // Swipe was too slow

                Vector2 touchEndPos = Touchscreen.current.primaryTouch.position.ReadValue();
                Vector2 delta = touchEndPos - _touchStartPos;

                if (delta.magnitude < minSwipeDistance) return; // Swipe was too short

                DetectSwipe(delta);
            }
        }

        private void DetectSwipe(Vector2 delta)
        {
            // Use the larger axis of movement to determine swipe direction
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                // Horizontal Swipe
                OnSwipe?.Invoke(delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left);
            }
            else
            {
                // Vertical Swipe
                OnSwipe?.Invoke(delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down);
            }
        }
    }
}
