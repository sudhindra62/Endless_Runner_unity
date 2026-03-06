using UnityEngine;

public class ImpactBurst : MonoBehaviour
{
    void Start()
    {
        // Example: Play a particle effect and a sound
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
    }
}
