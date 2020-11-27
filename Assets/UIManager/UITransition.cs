using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// Base class for Transition implementation
    /// Transition are animations that can be done when opening or closing a screen
    /// They are just monobehaviours (so they have to be added on a gameobject in scene
    /// and they are linked on in Transition Out Transition fields of ScreenControllers
    /// By default screen gets an in Transition used when opening
    /// and an out Transition when closing window
    /// You can inherite from this class to make your own transition
    /// For instance as in the sample directory you can do a FadeTransition that fades the screen
    /// </summary>
    public class UITransition : MonoBehaviour
    {
        /// <summary>
        /// Plays the transition
        /// It is the only entry point from transitions
        /// </summary>
        /// <param name="animatedTransform">The transform to move</param>
        /// <param name="onFinished">The action to call when finished. used by ScreenController to know when transition is done</param>
        public virtual void Play(Transform animatedTransform, Action onFinished)
        {
            // call the callback if exists
            onFinished?.Invoke();
        }
    }
}
