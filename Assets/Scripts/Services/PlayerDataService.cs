
using UnityEngine;

public class PlayerDataService : MonoBehaviour
{
    private PlayerDataRepository _repository;
    private PlayerData_New _playerData;

    private void Awake()
    {
        _repository = new PlayerDataRepository();
        _playerData = _repository.Load();
    }

    public PlayerData_New GetPlayerData()
    {
        return _playerData;
    }

    public void SetPlayerData(PlayerData_New data)
    {
        _playerData = data;
        _repository.Save(_playerData);
    }
}
