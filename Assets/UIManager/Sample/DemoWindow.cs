using System;
using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Custom parameters to have buttons that launch panel 1 and panel 2 in order to disable them
/// while panels are open
/// </summary>
[Serializable]
public class DemoWindowParameters : UIWindowParameters
{
    public Button panel1Button;
    public Button panel2Button;
}

/// <summary>
/// The demowindow is a window with custom parameters DemoWindowParameters
/// </summary>
public class DemoWindow : UIWindow<DemoWindowParameters>
{
    /// <summary>
    /// this is called by Unity when Click on OpenPanel1 button
    /// </summary>
    public void OnClickShowPanel1()
    {
        // disable the corresponding button
        Parameters.panel1Button.enabled = false;
        // add a listener to the OnPanelClosedEvent (OnPanel1Closed)
        Events.Instance.TypeOf<DemoPanel.OnPanelClosedEvent>().AddListener(OnPanel1Closed);
        // open the demoPanel
        UIController.Instance.ShowPanel("DemoPanel");
    }

    /// <summary>
    /// This is called when open panel 2 button is pressed by  UnityEvents
    /// It use the same event as panel 1 but the listener is different
    /// </summary>
    public void OnClickShowPanel2()
    {
        // disable corresponding button
        Parameters.panel2Button.enabled = false;
        // register another function for the OnPanelClosedEvent (OnPanel2Close)
        Events.Instance.TypeOf<DemoPanel.OnPanelClosedEvent>().AddListener(OnPanel2Closed);
        // show the panel2
        UIController.Instance.ShowPanel("DemoPanel2");
    }

    /// <summary>
    /// Call by DemoPanel script when dispatching events
    /// </summary>
    /// <param name="panel">The panel that throw the event</param>
    void OnPanel1Closed(DemoPanel panel)
    {
        // we can now remove the listener for this fonction
        Events.Instance.TypeOf<DemoPanel.OnPanelClosedEvent>().RemoveListener(OnPanel1Closed);
        // enable back the button
        Parameters.panel1Button.enabled = true;
    }

    /// <summary>
    /// Called by demoPanel script when dispatching event
    /// </summary>
    /// <param name="panel"></param>
    void OnPanel2Closed(DemoPanel panel)
    {
        // remove the listener
        Events.Instance.TypeOf<DemoPanel.OnPanelClosedEvent>().RemoveListener(OnPanel2Closed);
        // enable back the button
        Parameters.panel2Button.enabled = true;
    }

    /// <summary>
    /// Called by unity when Close button is pressed
    /// </summary>
    public void OnClickClose()
    {
        // show popup
        UIController.Instance.OpenWindow("PopupWindow");
    }
}
