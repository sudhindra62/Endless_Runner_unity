
using UnityEngine;

namespace EndlessRunner.Themes
{
    [CreateAssetMenu(fileName = "ThemeData", menuName = "EndlessRunner/Theme Data")]
    public class ThemeData : ScriptableObject
    {
        [Header("Environment")]
        public GameObject[] environmentPrefabs;
        public Material skybox;

        [Header("Lighting")]
        public Light sun;
        public Color dayLightColor = Color.white;
        public Color nightLightColor = Color.black;

        [Header("Obstacles")]
        public GameObject[] obstaclePrefabs;

        [Header("Collectibles")]
        public GameObject coinModel;

        [Header("Characters")]
        public GameObject enemyChaser;

        [Header("UI")]
        public Color uiAccentColor = Color.white;

        [Header("Audio")]
        public AudioClip music;
    }
}
