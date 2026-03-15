
using UnityEngine;

namespace EndlessRunner.Themes
{
    [CreateAssetMenu(fileName = "Theme", menuName = "Endless Runner/Theme")]
    public class ThemeSO : ScriptableObject
    {
        public string themeName;
        public GameObject[] environmentPrefabs;
        public Material skyboxMaterial;
    }
}
