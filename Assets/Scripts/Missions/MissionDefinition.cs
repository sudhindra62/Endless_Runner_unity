
using UnityEngine;

    [CreateAssetMenu(fileName = "New Mission", menuName = "Endless Runner/Mission Definition")]
    public class MissionDefinition : ScriptableObject
    {
        public string description;
        public MissionType type;
        public float target;

        public Mission CreateMission()
        {
            return new Mission(name, description, type, target);
        }
    }

