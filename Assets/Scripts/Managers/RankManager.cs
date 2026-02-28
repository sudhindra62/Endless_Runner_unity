using UnityEngine;
using System.Collections.Generic;

public class RankManager : MonoBehaviour
{
    [SerializeField] private List<RankData> ranks;
    private PlayerDataManager _playerDataManager;

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        if (_playerDataManager == null)
        { 
            Debug.LogError("PlayerDataManager not found!");
        }
    }

    public RankData GetCurrentRank()
    {
        if (_playerDataManager == null) return null;

        int playerLevel = _playerDataManager.Level;
        RankData currentRank = null;

        foreach (var rank in ranks)
        {
            if (playerLevel >= rank.levelRequirement)
            {
                currentRank = rank;
            }
            else
            {
                break;
            }
        }
        return currentRank;
    }
}
