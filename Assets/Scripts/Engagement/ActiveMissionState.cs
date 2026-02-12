using System;

[Serializable]
public class ActiveMissionState
{
    public ProjectMissionData Definition;

    public float GetProgress01()
    {
        if (Definition == null || Definition.goal == 0)
        {
            return 0;
        }
        return (float)Definition.progress / Definition.goal;
    }
}
