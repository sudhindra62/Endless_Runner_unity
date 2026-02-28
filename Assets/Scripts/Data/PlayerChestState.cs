using System;

[Serializable]
public class PlayerChestState
{
    public string chestId;
    public DateTime lastOpenedTime;
    public bool isReadyToCollect;
}
