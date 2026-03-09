
using UnityEngine;
using Core;
using Managers;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Movement")]
        [SerializeField] private float forwardSpeed = 15f;
        [SerializeField] private float laneSwitchSpeed = 10f;
        [SerializeField] private float laneWidth = 4f;

        private Rigidbody _rb;
        private int _currentLane = 0;
        private Vector3 _targetPosition;
        private Vector3 _initialPosition;

        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSwipe += HandleSwipe;
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSwipe -= HandleSwipe;
            }
        }

        private void Start()
        {
            _targetPosition = _initialPosition;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, forwardSpeed);

            if (transform.position.x != _targetPosition.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_targetPosition.x, transform.position.y, transform.position.z), laneSwitchSpeed * Time.deltaTime);
            }
        }

        private void HandleSwipe(SwipeDirection direction)
        {
            if (direction == SwipeDirection.Left && _currentLane > -1)
            {
                _currentLane--;
            }
            else if (direction == SwipeDirection.Right && _currentLane < 1)
            {
                _currentLane++;
            }

            _targetPosition.x = _currentLane * laneWidth;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                GameEvents.TriggerPlayerDied();
                enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                GameEvents.TriggerCoinCollected(1);
                other.gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            transform.position = _initialPosition;
            _targetPosition = _initialPosition;
            _currentLane = 0;
            _rb.velocity = Vector3.zero;
            enabled = true;
        }
    }
}
