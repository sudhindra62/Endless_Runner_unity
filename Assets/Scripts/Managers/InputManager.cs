
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Core;

namespace Managers
{
    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Manages player input using the new Input System.
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        public event Action<SwipeDirection> OnSwipe;

        [Header("Input Configuration")]
        [Tooltip("The minimum distance for a swipe to be registered.")]
        [SerializeField] private float minSwipeDistance = 20f;

        private PlayerInput _playerInput;
        private Vector2 _startPosition;

        protected override void Awake()
        {
            base.Awake();
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.Touch.PrimaryContact.started += OnTouchStart;
            _playerInput.Touch.PrimaryContact.canceled += OnTouchEnd;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            _playerInput.Touch.PrimaryContact.started -= OnTouchStart;
            _playerInput.Touch.PrimaryContact.canceled -= OnTouchEnd;
        }

        private void OnTouchStart(InputAction.CallbackContext context)
        {
            _startPosition = _playerInput.Touch.PrimaryPosition.ReadValue<Vector2>();
        }

        private void OnTouchEnd(InputAction.CallbackContext context)
        {
            Vector2 endPosition = _playerInput.Touch.PrimaryPosition.ReadValue<Vector2>();
            DetectSwipe(endPosition);
        }

        private void DetectSwipe(Vector2 endPosition)
        {
            Vector2 delta = endPosition - _startPosition;

            if (delta.magnitude < minSwipeDistance) return;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                OnSwipe?.Invoke(delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left);
            }
            else
            {
                OnSwipe?.Invoke(delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down);
            }
        }
    }
}
