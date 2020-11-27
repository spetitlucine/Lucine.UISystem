using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// Interface that a screen must implement
    /// </summary>
    public interface IUIScreenController 
    {
        /// <summary>
        /// a screen should have an id - it is automatically setup during layer initialization
        /// it contains the name of the gameobject root of the screen (which has the screencontroller)
        /// </summary>
        string ScreenId { get; set; }

        /// <summary>
        /// A flag indicating if the screen is displayed or not
        /// </summary>
        bool IsVisible { get; }
        
        /// <summary>
        /// Show the screen giving it eventually a parameter to set
        /// </summary>
        /// <param name="parameters">optional parameter implementing IUISCreenParameters. If not set use default value as defined in screencontroller</param>
        void Show(IUIScreenParameters parameters = null);
        
        /// <summary>
        /// Hide the screen by default using a Transition if defined in IUIScreenParameters 
        /// </summary>
        /// <param name="animate">use UITransition if defined if true, else bypass UITransition</param>
        void Hide(bool animate = true);

        /// <summary>
        /// Triggers events on registred listeners
        /// OnInTransitionFinished sent when in transition is finished
        /// OnOutTransitionFinished sent when out transition is finished
        /// OnCloseRequest sent when close is asked
        /// OnScreenDestroyed sent when the screen is destroyed
        /// </summary>
        Action<IUIScreenController> OnInTransitionFinished { get; set; }
        Action<IUIScreenController> OnOutTransitionFinished { get; set; }
        Action<IUIScreenController> OnCloseRequest { get; set; }
        Action<IUIScreenController> OnScreenDestroyed { get; set; }
    }
    
}
