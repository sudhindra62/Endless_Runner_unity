
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float lifetime = 2f;

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), lifetime);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
