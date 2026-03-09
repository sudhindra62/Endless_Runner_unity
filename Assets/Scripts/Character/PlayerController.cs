
using UnityEngine;

/// <summary>
/// Handles player input and translates it into actions for the CharacterMotor.
/// This is the primary interface between the player and the character.
/// Created by Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour
{
    private CharacterMotor motor;

    // Input detection
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private float swipeThreshold = 50f; // Minimum distance for a swipe

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    void Update()
    {
        // --- KEYBOARD INPUT (for editor testing) ---
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            motor.Jump();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            motor.Slide();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            motor.ChangeLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            motor.ChangeLane(1);
        }

        // --- TOUCH INPUT ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isSwiping = true;
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isSwiping)
                    {
                        Vector2 swipeDelta = touch.position - touchStartPos;
                        if (Mathf.Abs(swipeDelta.x) > swipeThreshold || Mathf.Abs(swipeDelta.y) > swipeThreshold)
                        {
                            HandleSwipe(swipeDelta);
                        }
                    }
                    isSwiping = false;
                    break;
            }
        }
    }

    private void HandleSwipe(Vector2 swipeDelta)
    {
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            // Horizontal swipe
            if (swipeDelta.x > 0)
            {
                motor.ChangeLane(1); // Right
            }
            else
            {
                motor.ChangeLane(-1); // Left
            }
        }
        else
        {
            // Vertical swipe
            if (swipeDelta.y > 0)
            {
                motor.Jump();
            }
            else
            {
                motor.Slide();
            }
        }
    }
}
