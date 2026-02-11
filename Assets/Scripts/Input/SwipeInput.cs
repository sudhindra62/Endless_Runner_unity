using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    [Header("Reference")]
    public PlayerController player;

    Vector2 startTouch;
    bool swiping;

    void Update()
    {
        // Allow keyboard in editor, swipe on mobile
        if (Application.isEditor)
            return;

        if (Input.touchCount == 0 || player == null)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouch = touch.position;
            swiping = true;
        }
        else if (touch.phase == TouchPhase.Ended && swiping)
        {
            Vector2 delta = touch.position - startTouch;
            swiping = false;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0) player.ChangeLane(1);
                else player.ChangeLane(-1);
            }
            else
            {
                if (delta.y > 0) player.Jump();
                else player.Slide();
            }
        }
    }
}
