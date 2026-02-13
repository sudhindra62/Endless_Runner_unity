
using UnityEngine;

public class CoinDoubler : MonoBehaviour
{
    public static CoinDoubler instance;

    public bool isDoublerActive = false;
    public int multiplier = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateDoubler()
    {
        isDoublerActive = true;
    }

    public void DeactivateDoubler()
    {
        isDoublerActive = false;
    }
}
