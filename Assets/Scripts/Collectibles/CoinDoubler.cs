
using UnityEngine;
using System.Collections;

public class CoinDoubler : MonoBehaviour
{
    [Tooltip("The duration in seconds for which the coin doubler effect is active.")]
    [SerializeField] private float duration = 10f;

    public static event System.Action<bool> OnCoinDoublerStatusChanged;

    public bool IsActive { get; private set; }

    private Coroutine activeCoroutine;

    public void Activate()
    {
        if (IsActive)
        {
            // If already active, just restart the timer
            StopCoroutine(activeCoroutine);
        }

        IsActive = true;
        OnCoinDoublerStatusChanged?.Invoke(IsActive);
        activeCoroutine = StartCoroutine(DeactivationCoroutine());
        
        Debug.Log("Coin Doubler has been activated.");
    }

    private IEnumerator DeactivationCoroutine()
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
    }

    private void Deactivate()
    {
        IsActive = false;
        OnCoinDoublerStatusChanged?.Invoke(IsActive);
        Debug.Log("Coin Doubler has been deactivated.");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var powerupManager = ServiceLocator.Get<PowerUpManager>();
            if(powerupManager != null)
            {
                powerupManager.ActivateCoinDoubler();
                gameObject.SetActive(false); // Disable after collection
            }
        }
    }
}
