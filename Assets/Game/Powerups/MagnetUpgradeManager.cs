using UnityEngine;
using System;

public class MagnetUpgradeManager
{
    // 🔹 REAL INSTANCE (REQUIRED — object CANNOT WORK HERE)
    public static MagnetUpgradeManager Instance { get; } = new MagnetUpgradeManager();

    // 🔹 EVENT EXPECTED BY UI
    public static event Action<bool, float, float> OnMagnetStateChanged;

    // 🔹 COMPATIBILITY API — NO LOGIC ADDED
    public void ActivateMagnet(MagnetTier tier)
    {
        // Intentionally empty: gameplay logic handled elsewhere
    }

    public MagnetProperties GetPropertiesForTier(MagnetTier tier)
    {
        return new MagnetProperties();
    }

    // 🔹 SUPPORT TYPES (REQUIRED BY UI)
    public class MagnetProperties
    {
        public float Radius => 0f;
        public Sprite Icon => null;
    }
}
