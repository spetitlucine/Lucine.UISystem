using System;
using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class DemoWindowParameters : UIWindowParameters
{
    public Button panel1Button;
    public Button panel2Button;
}

public class DemoWindow : UIWindow<DemoWindowParameters>
{
    public void OnClickShowPanel1()
    {
        Parameters.panel1Button.enabled = false;
        Events.Instance.Get<DemoPanel.OnPanelClosedEvent>().AddListener(OnPanelClosed);
        UIController.Instance.ShowPanel("DemoPanel");
    }

    public void OnClickShowPanel2()
    {
        Parameters.panel2Button.enabled = false;
        Events.Instance.Get<DemoPanel.OnPanelClosedEvent>().AddListener(OnPanelClosed);
        UIController.Instance.ShowPanel("DemoPanel2");
        
    }

    void OnPanelClosed(DemoPanel panel)
    {
        Events.Instance.Get<DemoPanel.OnPanelClosedEvent>().RemoveListener(OnPanelClosed);

        switch (panel.ScreenId)
        {
            case "DemoPanel":
                Parameters.panel1Button.enabled = true;
                break;
            case "DemoPanel2":
                Parameters.panel2Button.enabled = true;
                break;
        }
    }
    
   
    public void OnClickClose()
    {
        UIController.Instance.OpenWindow("PopupWindow");
    }
}
