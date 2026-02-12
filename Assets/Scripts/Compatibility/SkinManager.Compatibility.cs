using System.Collections.Generic;

/// <summary>
/// Compatibility layer for legacy UI / Shop systems.
/// Does NOT own data. Delegates to SkinManager.
/// </summary>
public partial class SkinManager
{
    // Legacy-friendly wrapper
    public List<SkinData> GetAllSkinsCompat()
    {
        return GetAllSkins();
    }
}
