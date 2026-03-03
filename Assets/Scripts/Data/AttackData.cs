
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Gameplay/Attack Data")]
public class AttackData : ScriptableObject
{
    [Tooltip("The animation trigger for this attack.")]
    public string animationTrigger;

    [Tooltip("The damage this attack deals.")]
    public int damage = 10;

    [Tooltip("The range of this attack.")]
    public float range = 1.5f;

    [Tooltip("The cooldown of this attack in seconds.")]
    public float cooldown = 0.5f;
}
