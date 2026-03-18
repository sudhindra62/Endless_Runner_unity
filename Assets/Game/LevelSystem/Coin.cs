using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isMoving = false;
    private Transform target;
    private float moveSpeed;

    void Update()
    {
        if (isMoving && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    public void MoveTowards(Transform target, float speed)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.isMoving = true;
    }
}
