using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    public float magnetRadius = 5f;
    public float magnetForce = 10f;
    public LayerMask coinLayer;

    private bool isMagnetActive = false;

    void Update()
    {
        if (isMagnetActive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, magnetRadius, coinLayer);
            foreach (var hitCollider in hitColliders)
            {
                Coin coin = hitCollider.GetComponent<Coin>();
                if (coin != null)
                {
                    coin.MoveTowards(transform, magnetForce);
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        isMagnetActive = active;
    }
}
