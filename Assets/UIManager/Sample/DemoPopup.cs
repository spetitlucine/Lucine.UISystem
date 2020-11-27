using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;

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
    }

    public void OnClickOnCancel()
    {
        UIController.Instance.CloseWindow("PopupWindow");        
    }
}
