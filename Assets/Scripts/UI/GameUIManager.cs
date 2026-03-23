
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
            OnUIStateChanged?.Invoke(MapState(panelType));
        }
    }

    public void HidePanel(UIPanelType panelType)
    {
        if (_uiPanels.TryGetValue(panelType, out UIPanel panel))
        {
            panel.Hide();
            OnPanelClosed?.Invoke(panelType);
            OnUIStateChanged?.Invoke(UIState.None);
        }
    }

    private UIState MapState(UIPanelType panelType)
    {
        switch (panelType)
        {
            case UIPanelType.MainMenu: return UIState.MainMenu;
            case UIPanelType.InGame: return UIState.InGame;
            case UIPanelType.Pause: return UIState.Paused;
            case UIPanelType.GameOver: return UIState.GameOver;
            case UIPanelType.Settings: return UIState.Settings;
            default: return UIState.None;
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
