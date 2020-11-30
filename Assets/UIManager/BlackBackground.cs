using System.Collections;
using System.Collections.Generic;
using Lucine.UISystem;
using UnityEngine;

/// <summary>
/// This class helps to handle black background for popups
/// </summary>
public class BlackBackground : MonoBehaviour
{
    /// <summary>
    /// Show the associated game object and set it to lastsibling
    /// Because it is showed before the popup the popup will be later just below this gameobject
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
        
    }

    /// <summary>
    /// Hide the blackbackground
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
