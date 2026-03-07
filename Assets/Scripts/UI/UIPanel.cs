
using UnityEngine;

/// <summary>
/// Base class for all UI panels.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public abstract class UIPanel : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
