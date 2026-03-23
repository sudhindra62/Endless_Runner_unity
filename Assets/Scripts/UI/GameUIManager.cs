
using UnityEngine;
using System.Collections.Generic;

public class GameUIManager : Singleton<GameUIManager>
{
    public static event System.Action<UIPanelType> OnPanelOpened;
    public static event System.Action<UIPanelType> OnPanelClosed;
    public static event System.Action<UIState> OnUIStateChanged;
    
    private Dictionary<UIPanelType, UIPanel> _uiPanels = new Dictionary<UIPanelType, UIPanel>();

    public void RegisterPanel(UIPanel panel)
    {
        if (!_uiPanels.ContainsKey(panel.PanelType))
        {
            _uiPanels.Add(panel.PanelType, panel);
        }
    }

    public void ShowPanel(UIPanelType panelType)
    {
        if (_uiPanels.TryGetValue(panelType, out UIPanel panel))
        {
            panel.Show();
            OnPanelOpened?.Invoke(panelType);
        }
    }

    public void HidePanel(UIPanelType panelType)
    {
        if (_uiPanels.TryGetValue(panelType, out UIPanel panel))
        {
            panel.Hide();
            OnPanelClosed?.Invoke(panelType);
        }
    }
}

public enum UIPanelType
{
    MainMenu,
    InGame,
    Pause,
    GameOver,
    Settings,
    Tutorial // ADDED: Missing value referenced in errors
}
