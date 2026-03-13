
using System;

public static class GameEvents
{
    public static event Action<bool> OnRunComplete;
    public static void RunComplete(bool noRevive) => OnRunComplete?.Invoke(noRevive);

    public static event Action<int> OnScoreIncreased;
    public static void ScoreIncreased(int amount) => OnScoreIncreased?.Invoke(amount);

    public static event Action<int> OnCoinCollected;
    public static void CoinCollected(int amount) => OnCoinCollected?.Invoke(amount);

    public static event Action OnBossDefeated;
    public static void BossDefeated() => OnBossDefeated?.Invoke();

    public static event Action OnPowerUpUsed;
    public static void PowerUpUsed() => OnPowerUpUsed?.Invoke();

    public static event Action OnNearMiss;
    public static void NearMiss() => OnNearMiss?.Invoke();

    public static event Action OnReviveUsed;
    public static void ReviveUsed() => OnReviveUsed?.Invoke();

    public static event Action OnLogin;
    public static void Login() => OnLogin?.Invoke();
}
