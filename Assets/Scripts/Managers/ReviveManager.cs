using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ReviveManager : MonoBehaviour
{
    public static ReviveManager Instance { get; private set; }

    private int reviveUsedThisRun = 0;
    private readonly Dictionary<string, int> externalRevives = new Dictionary<string, int>();

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

    public void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    public void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    public bool CanRevive()
    {
        int totalExtraRevives = externalRevives.Values.Sum();
        return reviveUsedThisRun < totalExtraRevives;
    }

    public void UseRevive()
    {
        if (CanRevive())
        {
            reviveUsedThisRun++;
        }
    }

    public void ApplyExtraRevives(string sourceId, int count)
    {
        externalRevives[sourceId] = count;
    }

    public void RemoveExtraRevives(string sourceId)
    {
        externalRevives.Remove(sourceId);
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            reviveUsedThisRun = 0; // Reset on a new run
        }
        else if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            externalRevives.Clear();
        }
    }
}
