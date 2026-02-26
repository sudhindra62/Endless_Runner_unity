using UnityEngine;

public class SpeedDifficultyActivator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("Speed Scaling")]
    public float speedIncreasePerSecond = 0.02f;
    public float maxSpeed = 18f;

    private float baseSpeed;

    private void Start()
    {
        if (playerController != null)
            baseSpeed = playerController.forwardSpeed;
    }

    private void Update()
    {
        if (playerController == null) return;
        if (playerController.IsDead) return;

        float newSpeed = baseSpeed + (Time.timeSinceLevelLoad * speedIncreasePerSecond);
        playerController.forwardSpeed = Mathf.Min(newSpeed, maxSpeed);
    }
}