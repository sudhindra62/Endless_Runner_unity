
using System;
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Input
{
    /// <summary>
    /// A centralized manager for handling all player input.
    /// This abstracts the input source (keyboard, touch, etc.) from the player logic.
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        // Events for other scripts to subscribe to
        public event Action<int> OnLaneChange; // -1 for left, 1 for right
        public event Action OnJump;

        // For swipe detection
        private Vector2 touchStartPosition;
        private float swipeThreshold = 50f;

        void Update()
        {
            // --- Keyboard Input ---
            if (UnityEngine.Input.GetKeyDown(KeyCode.A) || UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnLaneChange?.Invoke(-1);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D) || UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnLaneChange?.Invoke(1);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnJump?.Invoke();
            }

            // --- Touch Input (Swipe) ---
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 touchEndPosition = touch.position;
                    float swipeDistanceX = touchEndPosition.x - touchStartPosition.x;
                    float swipeDistanceY = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(swipeDistanceX) > swipeThreshold || Mathf.Abs(swipeDistanceY) > swipeThreshold)
                    {
                        // Prioritize horizontal swipes over vertical ones
                        if (Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY))
                        {
                            // Horizontal Swipe
                            if (swipeDistanceX > 0)
                            {
                                OnLaneChange?.Invoke(1); // Right
                            }
                            else
                            {
                                OnLaneChange?.Invoke(-1); // Left
                            }
                        }
                        else
                        {
                            // Vertical Swipe (for jump)
                            if (swipeDistanceY > 0)
                            {
                                OnJump?.Invoke();
                            }
                        }
                    }
                }
            }
        }
    }
}
