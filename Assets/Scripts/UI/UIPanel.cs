
using UnityEngine;

/// <summary>
/// A base class for all UI panels, providing standardized show/hide functionality.
/// This foundational element was created by Supreme Guardian Architect v12.
/// </summary>
public abstract class UIPanel : MonoBehaviour
{
    /// <summary>
    /// Activates the panel's GameObject, making it visible.
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates the panel's GameObject, hiding it from view.
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
