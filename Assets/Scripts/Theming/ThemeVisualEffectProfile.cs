using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ThemeVisualEffectProfile", menuName = "Theming/Theme VFX Profile")]
public class ThemeVisualEffectProfile : ScriptableObject
{
    [Header("Particle Overrides")]
    public GameObject CoinCollectParticle;
    public GameObject PowerUpCollectParticle;
    public GameObject PlayerDeathParticle;
}
