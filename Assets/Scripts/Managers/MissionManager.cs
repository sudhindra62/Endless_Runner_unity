
using System;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    public event Action OnMissionProgress;

    void Update()
    {
        // Placeholder for mission progress logic
    }
}
