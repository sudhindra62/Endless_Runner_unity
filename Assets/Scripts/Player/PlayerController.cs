
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnDeath;

    [Header("Configuration")]
    [SerializeField] private float reviveImmunityTime = 2f;

    private PlayerMotor motor;
    private bool isDead = false;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isDead) return;

        // In a real game, you would check for a specific tag or layer
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        motor.Stop();
        OnDeath?.Invoke();
    }

    public void Revive()
    {
        isDead = false;
        motor.ResetVelocity();
        StartCoroutine(ActivateImmunity());
    }

    private IEnumerator ActivateImmunity()
    {
        motor.SetImmune(true);
        yield return new WaitForSeconds(reviveImmunityTime);
        motor.SetImmune(false);
    }
}
