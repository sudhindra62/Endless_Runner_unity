
using UnityEngine;

namespace EndlessRunner.Themes
{
    [CreateAssetMenu(fileName = "ThemeSO", menuName = "EndlessRunner/ThemeSO", order = 0)]
    public class ThemeSO : ScriptableObject
    {
        public string themeName;
        public Material groundMaterial;
        public Material obstacleMaterial;
        public Color skyColor;
    }
}
