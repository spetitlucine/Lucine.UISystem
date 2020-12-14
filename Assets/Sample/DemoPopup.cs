using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;

/// <summary>
/// Demo popup is a basic window (inside unity we set its ispopup parameter)
/// </summary>
public class DemoPopup : UIWindow
{
    // Start is called before the first frame update
    protected override void Start()
    {
    }

    /// <summary>
    /// Unity will call this with its event system when the ok button is pressed.
    /// We hide all panels and windows and dispatch the ApplicationQuitEvent defined in UIStarter.cs
    /// </summary>
    public void OnClickOnOk()
    {
        UIController.Instance.HideAllPanels();
        UIController.Instance.CloseAllWindows();
        Events.Instance.TypeOf<ApplicationQuitEvent>().Dispatch();
    }

    /// <summary>
    /// Unity will call this when cancel button is pressed.
    /// Only hide the popup, no need to restore anything.
    /// </summary>
    public void OnClickOnCancel()
    {
        UIController.Instance.CloseWindow("PopupWindow");        
    }
}
