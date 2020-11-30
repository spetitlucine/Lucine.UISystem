using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;

/// <summary>
/// Demo popup is a basic window (inside unity we set its ipopup parameter)
/// </summary>
public class DemoPopup : UIWindow
{
    // Start is called before the first frame update
    protected override void Start()
    {
    }

    public void OnClickOnOk()
    {
        UIController.Instance.HideAllPanels();
        UIController.Instance.CloseAllWindows();
        Events.Instance.TypeOf<ApplicationQuitEvent>().Dispatch();
    }

    public void OnClickOnCancel()
    {
        UIController.Instance.CloseWindow("PopupWindow");        
    }
}
