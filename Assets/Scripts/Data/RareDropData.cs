using System;

// A simple data class to represent a rare drop.
[Serializable]
public class RareDropData 
{
    public string itemID;
    public string rarity;
    public float baseChance;
    public float pityIncrease;
}