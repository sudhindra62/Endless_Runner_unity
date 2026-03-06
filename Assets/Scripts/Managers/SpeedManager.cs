
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float maxSpeed = 30f;
    public float speedIncreaseRate = 0.1f;

    private float currentSpeed;

    void Start()
    {
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
