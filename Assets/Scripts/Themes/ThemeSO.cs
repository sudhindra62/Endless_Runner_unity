
using UnityEngine;
using EndlessRunner.Chasers;

namespace EndlessRunner.Themes
{
    public enum ThemeUnlockType
    {
        Free,
        GemUnlock,
        PremiumSubscription
    }

    [CreateAssetMenu(fileName = "ThemeSO", menuName = "EndlessRunner/ThemeSO", order = 0)]
    public class ThemeSO : ScriptableObject
    {
        [Header("Theme Info")]
        public string themeName;
        public GameObject themePreviewPrefab;

        [Header("Unlock Conditions")]
        public ThemeUnlockType unlockType;
        public int gemPrice;

        [Header("Visuals")]
        public Material skybox;
        public Color lightingColor = Color.white;
        public float lightingIntensity = 1.0f;
        public Color fogColor = Color.grey;
        public float fogDensity = 0.01f;

        [Header("Game Element Prefabs")]
        public GameObject[] environmentModules;
        public GameObject coinPrefab;
        public Chaser chaser;

        [Header("Materials")]
        public Material groundMaterial;
        public Material obstacleMaterial;

        [Header("Audio")]
        public AudioClip backgroundMusic;
        
        [Header("UI")]
        public Color uiAccentColor = Color.white;
    }
}
