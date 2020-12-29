using System.Collections;
using System.Collections.Generic;
using Lucine.UISystem;
using UnityEditor;
using UnityEngine;

public class ScreenTestParameter : IUIScreenParameters
{
    
}
public class ScreenTest : UIScreenController<ScreenTestParameter>
{
    public void Close()
    {
        Hide();
        OnCloseRequest(this);        
    }
}
