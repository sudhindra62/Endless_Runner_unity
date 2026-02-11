using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    public void Collect()
    {
        ScoreManager.instance.AddScore(value);
        Destroy(gameObject);
    }
}
