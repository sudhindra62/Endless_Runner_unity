using UnityEngine;
using System.Collections.Generic;
using Skins;

[CreateAssetMenu(fileName = "SkinCatalog", menuName = "Skins/Skin Catalog")]
public class SkinCatalog : ScriptableObject
{
    public List<SkinData> skins;
}
