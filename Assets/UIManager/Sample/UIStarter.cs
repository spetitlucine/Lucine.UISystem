using System.Collections;
using System.Collections.Generic;
using Lucine.UISystem;
using UnityEngine;

public class UIStarter : MonoBehaviour
{
    public UIController uiController;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        uiController.OpenWindow("DemoWindow");
    }

}
