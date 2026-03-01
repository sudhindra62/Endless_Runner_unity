
using UnityEngine;
using System;

public class BossChaseManager : MonoBehaviour
{
    public static BossChaseManager Instance { get; private set; }

    public static event Action<float> OnBossChaseStart;
    public static event Action OnBossChaseEnd;

    [Header("Trigger Thresholds")]
    [SerializeField] private int scoreThreshold = 5000;
    [SerializeField] private int comboThreshold = 20;
    [SerializeField] private float distanceThreshold = 1000f;

    [Header("Chase Parameters")]
    [SerializeField] private float chaseDuration = 30f;
    [SerializeField] private float baseCooldownDuration = 120f;
    [SerializeField] private float outrunDistance = 50f;

    [Header("Boss Prefabs")]
    [SerializeField] public GameObject barrierPrefab;

    [Header("Pressure Modifiers")]
    [SerializeField] private float speedMultiplier = 1.1f;
    [SerializeField] private float obstacleFrequencyMultiplier = 1.2f;
    [SerializeField] private int scoreMultiplier = 2;
    [SerializeField] private float coinRewardMultiplier = 1.5f;

    [Header("Dependencies")]
    private BossChaseUI bossChaseUI;
    private ObjectPooler objectPooler;

    private ScoreManager scoreManager;
    private GameDifficultyManager difficultyManager;
    private PlayerMovement playerMovement;
    private FlowComboManager flowComboManager;
    private FeverModeManager feverModeManager;
    private RewardManager rewardManager;
    private DistanceTracker distanceTracker;

    private bool isChaseActive = false;
    private float lastChaseEndTime = -Mathf.Infinity;
    private GameObject bossInstance;
    private BossController bossController;
    private float chaseTimer;
    private float cooldownMultiplier = 1f;

    public float RemainingChaseTime => chaseTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        difficultyManager = FindObjectOfType<GameDifficultyManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        flowComboManager = FindObjectOfType<FlowComboManager>();
        feverModeManager = FindObjectOfType<FeverModeManager>();
        rewardManager = FindObjectOfType<RewardManager>();
        distanceTracker = FindObjectOfType<DistanceTracker>();
        bossChaseUI = FindObjectOfType<BossChaseUI>();
        objectPooler = ObjectPooler.Instance;

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        if (scoreManager != null) ScoreManager.OnScoreChanged += CheckScoreTrigger;
        if (flowComboManager != null) FlowComboManager.OnMultiplierChanged += CheckComboTrigger;
    }

    private void UnsubscribeFromEvents()
    {
        if (scoreManager != null) ScoreManager.OnScoreChanged -= CheckScoreTrigger;
        if (flowComboManager != null) FlowComboManager.OnMultiplierChanged -= CheckComboTrigger;
    }

    private void Update()
    {
        if (isChaseActive)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0)
            {
                EndChase(true);
            }

            if (bossController != null && bossController.GetDistanceFromPlayer() > outrunDistance)
            {
                EndChase(true);
            }
        } 
        else if (CanStartChase() && distanceTracker != null && distanceTracker.Distance >= distanceThreshold)
        {
            StartChase();
        }
    }

    private void CheckScoreTrigger(int currentScore)
    {
        if (CanStartChase() && currentScore >= scoreThreshold)
        {
            StartChase();
        }
    }

    private void CheckComboTrigger(int currentMultiplier)
    {
        if(CanStartChase() && currentMultiplier >= comboThreshold)
        {
            StartChase();
        }
    }

    private bool CanStartChase()
    {
        bool onCooldown = Time.time < lastChaseEndTime + (baseCooldownDuration * cooldownMultiplier);
        bool feverActive = feverModeManager != null && feverModeManager.IsFeverActive();
        
        return !isChaseActive && !onCooldown && !feverActive;
    }

    public void StartChase()
    {
        if (!CanStartChase()) return;

        isChaseActive = true;
        chaseTimer = chaseDuration;
        Debug.Log("Boss Chase Started!");

        if (objectPooler != null && barrierPrefab != null)
        {
            objectPooler.AddPool("Barrier", barrierPrefab, 5);
        }

        SpawnBoss();
        ApplyPressureModifiers();

        OnBossChaseStart?.Invoke(chaseDuration);
    }

    private void EndChase(bool survived)
    {
        if (!isChaseActive) return;

        isChaseActive = false;
        lastChaseEndTime = Time.time;
        Debug.Log("Boss Chase Ended!");

        RemovePressureModifiers();
        DespawnBoss();

        if (survived)
        {
            GrantReward();
            if(bossChaseUI != null) bossChaseUI.ShowRewardNotification("You escaped the boss and earned 100 coins!");
        }

        OnBossChaseEnd?.Invoke();
    }
    
    public void OnPlayerDied()
    {
        if (isChaseActive)
        {
            EndChase(false);
        }
    }

    public void SetCooldownMultiplier(float multiplier)
    {
        cooldownMultiplier = Mathf.Max(0.1f, multiplier);
    }

    private void SpawnBoss()
    {
        if (playerMovement != null)
        {
            Vector3 spawnPos = playerMovement.transform.position - playerMovement.transform.forward * 20;
            bossInstance = objectPooler.SpawnFromPool("Boss", spawnPos, Quaternion.identity);
            bossController = bossInstance.GetComponent<BossController>();
        }
    }

    private void DespawnBoss()
    {
        if (bossInstance != null)
        {
            bossInstance.SetActive(false);
            bossController = null;
        }
    }

    private void ApplyPressureModifiers()
    {
        if (playerMovement != null) playerMovement.ApplySpeedMultiplier("BossChase", speedMultiplier);
        if (difficultyManager != null) difficultyManager.ApplyDifficultyMultiplier("BossChase", obstacleFrequencyMultiplier);
        if (scoreManager != null) scoreManager.ApplyScoreMultiplier("BossChase", scoreMultiplier);
        if (rewardManager != null) rewardManager.ApplyCoinMultiplier("BossChase", coinRewardMultiplier);
    }

    private void RemovePressureModifiers()
    {
        if (playerMovement != null) playerMovement.RemoveSpeedMultiplier("BossChase");
        if (difficultyManager != null) difficultyManager.RemoveDifficultyMultiplier("BossChase");
        if (scoreManager != null) scoreManager.RemoveScoreMultiplier("BossChase");
        if (rewardManager != null) rewardManager.RemoveCoinMultiplier("BossChase");
    }

    private void GrantReward()
    {
        Debug.Log("Boss Chase Survived! Granting reward.");
        if (rewardManager != null)
        {
            rewardManager.GrantReward(new Reward("Boss Chase Survived!", RewardType.Coins, 100));
        }
    }
}
