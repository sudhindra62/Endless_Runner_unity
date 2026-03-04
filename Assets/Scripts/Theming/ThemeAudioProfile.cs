using UnityEngine;

[CreateAssetMenu(fileName = "ThemeAudioProfile", menuName = "Theming/Theme Audio Profile")]
public class ThemeAudioProfile : ScriptableObject
{
    [Header("Music")]
    public AudioClip MusicTrack;

    [Header("SFX Overrides")]
    public List<AudioClip> JumpSounds;
    public List<AudioClip> SlideSounds;
}
