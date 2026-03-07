
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float duration = 5f;
    public int maxHits = 3;

    private int currentHits;
    private float activeTime;

    void OnEnable()
    {
        currentHits = maxHits;
        activeTime = duration;
    }

    void Update()
    {
        if (activeTime > 0)
        {
            activeTime -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHits -= damage;
        if (currentHits <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
