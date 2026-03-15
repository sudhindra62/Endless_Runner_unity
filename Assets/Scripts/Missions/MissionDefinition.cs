
using UnityEngine;

namespace EndlessRunner.Missions
{
    [CreateAssetMenu(fileName = "New Mission", menuName = "Endless Runner/Mission Definition")]
    public class MissionDefinition : ScriptableObject
    {
        public string description;
        public MissionType type;
        public float target;

        public Mission CreateMission()
        {
            return new Mission(description, type, target);
        }
    }
}
