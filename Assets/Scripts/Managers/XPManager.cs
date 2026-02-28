using UnityEngine;

public class XPManager : MonoBehaviour
{
    private PlayerDataManager _playerDataManager;

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        if (_playerDataManager == null)
        {
            Debug.LogError("PlayerDataManager not found!");
        }
    }

    public void AddXpForRun(int score)
    {
        if (_playerDataManager != null)
        {
            _playerDataManager.AddXPFromRun(score);
        }
    }
}
