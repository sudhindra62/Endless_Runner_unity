
using UnityEngine;

[System.Serializable]
public class ProductDefinition
{
    public string productId;
    public ProductType productType;
    public int amount;
}

public enum ProductType
{
    Gems,
    Coins
}
