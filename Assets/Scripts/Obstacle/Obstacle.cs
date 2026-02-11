using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPowerUp power = other.GetComponent<PlayerPowerUp>();
        PlayerController player = other.GetComponent<PlayerController>();

        if (power != null && power.HasShield())
        {
            power.BreakShield();
            Destroy(gameObject);
            return;
        }

        player.Die();
    }
}
