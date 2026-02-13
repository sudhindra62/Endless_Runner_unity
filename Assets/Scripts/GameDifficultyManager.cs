using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    [Header("Speed Settings")]
    public PlayerController player;
    public float speedIncreaseRate = 0.5f;
    public float maxSpeed = 25f;

    [Header("Rest Zone")]
    public ObstacleSpawner obstacleSpawner;
    public float restDuration = 5f;
    public float hardcoreDuration = 20f;

    float timer;
    bool inRestZone;

    void Update()
    {
        timer += Time.deltaTime;

        // Gradual speed increase
        if (!inRestZone && player.forwardSpeed < maxSpeed)
        {
            player.forwardSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // Hardcore → Rest switch
        if (!inRestZone && timer >= hardcoreDuration)
        {
            EnterRestZone();
        }
    }

    void EnterRestZone()
    {
        inRestZone = true;
        timer = 0f;
        obstacleSpawner.SetSpawning(false);
        Invoke(nameof(ExitRestZone), restDuration);
    }

    void ExitRestZone()
    {
        inRestZone = false;
        timer = 0f;
        obstacleSpawner.SetSpawning(true);
    }
}
