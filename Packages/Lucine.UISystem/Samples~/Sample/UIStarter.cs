using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;
using Event = Lucine.Helpers.Event;

/// Definition of an event for application quit. The dispatch of this event will be done in DemoPopup
public class ApplicationQuitEvent : Event { }

public class UIStarter : MonoBehaviour
{
    public UIController uiController;
    
    // Start is called before the first frame update
    void Start()
    {
        // the UIManager should be initialized before opening windows
        // so UIController script execution order is before other scripts
        uiController.OpenWindow("DemoWindow");
        
        // register to ApplicationQuitEvent
        Events.Instance.TypeOf<ApplicationQuitEvent>().AddListener(OnCloseMainWindow);
    }

    void OnCloseMainWindow()
    {
        // unsubscribe to ApplicationQuitEvent
        Events.Instance.TypeOf<ApplicationQuitEvent>().RemoveListener(OnCloseMainWindow);
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
          Application.Quit();
#endif
    }
}
