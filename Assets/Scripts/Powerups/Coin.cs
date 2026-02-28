
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this coin type.")]
    public string poolTag = "Coin";

    [Tooltip("The number of coins this collectible represents.")]
    public int value = 1;

    [Tooltip("The rotation speed of the coin for visual effect.")]
    public float rotationSpeed = 100f;

    private bool hasBeenCollected = false;
    private bool isAttracted = false;

    private void OnEnable()
    {
        hasBeenCollected = false;
        isAttracted = false;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;

        PlayerDataManager.Instance?.AddCoins(value);

        ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
    }

    public bool IsAttracted()
    {
        return isAttracted;
    }

    public void SetAttracted(bool attracted)
    {
        isAttracted = attracted;
    }
}
