
using UnityEngine;
using System.Collections.Generic;

public class GameUIManager : Singleton<GameUIManager>
{
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
        }
    }

    public void HidePanel(UIPanelType panelType)
    {
        if (_uiPanels.TryGetValue(panelType, out UIPanel panel))
        {
            panel.Hide();
        }
    }
}

public enum UIPanelType
{
    MainMenu,
    InGame,
    Pause,
    GameOver,
    Settings
}
