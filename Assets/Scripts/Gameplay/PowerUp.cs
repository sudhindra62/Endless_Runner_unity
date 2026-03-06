
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUps/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public float duration;
    public GameObject visualEffect;
    // Can be extended with more properties, e.g., effect type, intensity
}
