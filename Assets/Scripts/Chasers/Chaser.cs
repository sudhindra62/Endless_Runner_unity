
using UnityEngine;

namespace EndlessRunner.Chasers
{
    [CreateAssetMenu(fileName = "NewChaser", menuName = "EndlessRunner/Chaser")]
    public class Chaser : ScriptableObject
    {
        public string chaserName;
        public GameObject chaserPrefab;
    }
}
