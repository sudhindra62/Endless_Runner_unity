

    public enum MissionStatus { Available, InProgress, Completed, Claimed }

    /// <summary>
    /// Runtime state of a single mission for a player.
    /// </summary>
    [System.Serializable]
    public class MissionState
    {
        public string missionId;
        public float currentValue;
        public bool isCompleted;
        public MissionData data; // The mission definition
        public float progress;   // Current progress value
        public float target;     // Target to complete
        public bool isClaimed;   // Has reward been claimed?
        public bool isComplete => progress >= target;
        public MissionStatus status;
    }

