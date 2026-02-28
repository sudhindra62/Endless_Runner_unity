using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;

    private void Awake()
    {
        ServiceLocator.Register<PlayerController>(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerController>();
    }

    void Update()
    {
        // Handle player input and movement
    }
}
