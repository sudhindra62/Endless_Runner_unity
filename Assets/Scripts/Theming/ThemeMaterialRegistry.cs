using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ThemeMaterialRegistry", menuName = "Theming/Theme Material Registry")]
public class ThemeMaterialRegistry : ScriptableObject
{
    [Header("Track Materials")]
    public List<Material> TrackMaterials;

    [Header("Obstacle Materials")]
    public List<Material> ObstacleMaterials;
}
