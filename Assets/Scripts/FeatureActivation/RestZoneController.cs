using UnityEngine;

public class RestZoneController : MonoBehaviour
{
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private float restIntervalMin = 60f;
    [SerializeField] private float restIntervalMax = 120f;
    [SerializeField] private float restDuration = 5f;

    private float nextRestTime;
    private bool isResting;

    void Start()
    {
        ScheduleNextRest();
    }

    void Update()
    {
        if (obstacleSpawner == null) return;

        if (!isResting && Time.time >= nextRestTime)
        {
            StartCoroutine(RestCoroutine());
        }
    }

    private void ScheduleNextRest()
    {
        nextRestTime = Time.time + Random.Range(restIntervalMin, restIntervalMax);
    }

    private System.Collections.IEnumerator RestCoroutine()
    {
        isResting = true;

        obstacleSpawner.enabled = false;

        yield return new WaitForSeconds(restDuration);

        obstacleSpawner.enabled = true;

        ScheduleNextRest();
        isResting = false;
    }
}