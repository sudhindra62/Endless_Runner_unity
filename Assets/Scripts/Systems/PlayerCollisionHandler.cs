
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerPowerUpHandler powerUpHandler;
    private PlayerDeathHandler deathHandler;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        powerUpHandler = GetComponent<PlayerPowerUpHandler>();
        deathHandler = GetComponent<PlayerDeathHandler>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(hit.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.GetComponent<Coin>()?.Collect();
        }
        else if (other.CompareTag("Shield"))
        {
            powerUpHandler.ActivateShield();
            ObjectPooler.Instance.ReturnToPool("ShieldPowerUp", other.gameObject);        
        }
        else if (other.CompareTag("Magnet"))
        {
            powerUpHandler.ActivateMagnet();
            ObjectPooler.Instance.ReturnToPool("MagnetPowerUp", other.gameObject);       
        }
        else if (other.CompareTag("SpecialCollectible"))
        {
            other.GetComponent<SpecialCollectible>()?.Collect();
        }    
    }

    private void HandleObstacleCollision(GameObject obstacle)
    {
        if (powerUpHandler.IsShieldActive())
        {
            powerUpHandler.BreakShield();
            ObjectPooler.Instance.ReturnToPool("Obstacle", obstacle);
        }
        else
        {
            deathHandler.Die();
        }
    }
}
