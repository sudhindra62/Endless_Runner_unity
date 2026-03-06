
using UnityEngine;

public class PowerUpEffectsController : MonoBehaviour
{
    public static PowerUpEffectsController Instance { get; private set; }

    [SerializeField] private ParticleSystem fusionEffect;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayActivationEffect(PowerUp powerUp)
    {
        if (powerUp.visualEffect != null)
        {
            Instantiate(powerUp.visualEffect, transform.position, Quaternion.identity);
        }
    }

    public void PlayFusionEffect(PowerUp powerUp)
    {
        if (fusionEffect != null)
        {
            // The fusion effect could be customized based on the power-up
            Instantiate(fusionEffect, transform.position, Quaternion.identity);
        }
    }
}
