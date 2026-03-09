
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public abstract UIPanelType PanelType { get; }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Awake()
    {
        GameUIManager.Instance.RegisterPanel(this);
    }
}
