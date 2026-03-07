
using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public Transform GetCurrentWaypoint()
    {
        if (waypoints.Length == 0)
        {
            return null;
        }
        return waypoints[currentWaypointIndex];
    }

    public void GoToNextWaypoint()
    {
        if (waypoints.Length > 0)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
