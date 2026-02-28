
using UnityEngine;

public class CoinDoubler : MonoBehaviour
{
    public static CoinDoubler Instance { get; private set; }

    public bool IsDoublerActive { get; private set; } = false;
    public int multiplier = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[CoinDoubler] Another instance was found and destroyed.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ActivateDoubler()
    {
        IsDoublerActive = true;
    }

    public void DeactivateDoubler()
    {
        IsDoublerActive = false;
    }
}
