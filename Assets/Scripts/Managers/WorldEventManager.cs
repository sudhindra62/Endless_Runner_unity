using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public static WorldEventManager Instance { get; private set; }
    public static event System.Action<WorldEventData> OnEventActivated;
    public static event System.Action<WorldEventData> OnEventDeactivated;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void ActivateEvent(WorldEventData eventData)
    {
        OnEventActivated?.Invoke(eventData);
    }

    public void DeactivateEvent(WorldEventData eventData)
    {
        OnEventDeactivated?.Invoke(eventData);
    }
}
