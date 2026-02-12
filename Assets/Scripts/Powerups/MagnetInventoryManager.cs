using UnityEngine;
using System;

public class MagnetInventoryManager : MonoBehaviour
{
    public static MagnetInventoryManager Instance { get; private set; }

    // 🔹 EVENT — DECLARED ONCE (UI requires this)
    public static event Action<MagnetTier, int> OnMagnetInventoryChanged;

    [Header("UI Reference")]
    [SerializeField] private MagnetUI magnetUI;

    private int magnetCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // 🔹 ORIGINAL METHOD — KEPT
    public bool UseMagnet()
    {
        if (magnetCount <= 0)
            return false;

        if (PowerUpManager.Instance != null &&
            PowerUpManager.Instance.IsPowerUpActive(PowerUpType.Magnet))
        {
            return false;
        }

        magnetCount--;

        PowerUpManager.Instance?.ActivatePowerUp(
            PowerUpType.Magnet,
            GetMagnetDuration()
        );

        UpdateUI();

        // 🔹 ADDITIVE NOTIFICATION
        OnMagnetInventoryChanged?.Invoke(MagnetTier.Small, magnetCount);

        return true;
    }

    // 🔹 ORIGINAL METHOD — KEPT
    public void AddMagnets(int amount)
    {
        magnetCount += amount;
        UpdateUI();

        // 🔹 ADDITIVE NOTIFICATION
        OnMagnetInventoryChanged?.Invoke(MagnetTier.Small, magnetCount);
    }

    public int GetMagnetCount()
    {
        return magnetCount;
    }

    private float GetMagnetDuration()
    {
        return 10f;
    }

    private void UpdateUI()
    {
        magnetUI?.UpdateMagnetCount(magnetCount);
    }
}
