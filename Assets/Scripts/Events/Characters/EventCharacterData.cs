
using UnityEngine;

[System.Serializable]
public class EventCharacterData
{
    public string characterID;
    public string characterName;
    public GameObject characterPrefab;
    public AnimationClip runningAnimation;
    public AnimationClip idleAnimation;
    public string associatedCosmeticEffectID;
    public string specialAbilityDescription;
    public float specialAbilityValue;
    public string unlockMethod; // e.g., "EventCurrency", "IAP"
    public int price;
    public string currencyType; // e.g., "eventCurrency", "gems", "INR"
}
