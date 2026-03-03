
using UnityEngine;
using System.Collections.Generic;

public class AttackManager : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private List<AttackData> comboAttacks;
    [SerializeField] private float comboResetTime = 1.5f;

    private int comboIndex = 0;
    private float lastAttackTime;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    public void OnAttackInput()
    {
        if (Time.time > lastAttackTime + comboResetTime)
        {
            comboIndex = 0;
        }

        if (comboIndex < comboAttacks.Count)
        {
            AttackData attack = comboAttacks[comboIndex];
            animator.SetTrigger(attack.animationTrigger);
            // Additional logic for damage, VFX, etc., would go here.
            
            lastAttackTime = Time.time;
            comboIndex++;
        }
    }

    public void ResetCombo()
    {
        comboIndex = 0;
    }
}
