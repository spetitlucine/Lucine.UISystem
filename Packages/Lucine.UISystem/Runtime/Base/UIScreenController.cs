using System;
using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// UIScreen controller is the base class for all types of windows, panels, popup whatever u want
    /// It implements the IUIScreenController interface
    /// It is templated depending of the Parameters we want for the window
    /// Parameters are thing you can add to customize the parameter of the window
    /// </summary>
    /// <typeparam name="TParameters">The type of parameter for this screen. There are base parameters class for Windows and panel but you can extend them to add your own parameters</typeparam>
    public class UIScreenController<TParameters> : MonoBehaviour, IUIScreenController where TParameters : IUIScreenParameters
    {
        // UITransition for screen appears (can be null if no effect is wanted)
        // you can derive from UITransition to make your own effect such as fading, coming from top, left, right or bottom
        [SerializeField]
        private UITransition m_inTransition;
        private UITransition InTransition
        {
            get => m_inTransition;
            set => m_inTransition = value;
        }
        
        // UITransition for screen disappears (can be null if no effect is wanted)
        // you can derive from UITransition to make your own effect such as fading, coming from top, left, right or bottom
        [SerializeField]
        private UITransition m_outTransition;
        private UITransition OutTransition
        {
            get => m_outTransition;
            set => m_outTransition = value;
        }

        // callback needed to be defined layers will register to them to know things about screen state
        public Action<IUIScreenController> OnInTransitionFinished { get; set; }
        public Action<IUIScreenController> OnOutTransitionFinished { get; set; }
        public Action<IUIScreenController> OnCloseRequest { get; set; }
        public Action<IUIScreenController> OnScreenDestroyed { get; set; }

        // the parameters for the screen
        // there are default parameters from inherited windows and panel
        // you can even inherit from window and panels parameters to add your own parameters
        // it will be exposed in inspector if public
        [SerializeField]
        private TParameters m_Parameters;
        protected TParameters Parameters
        {
            get => m_Parameters;
            set => m_Parameters = value;
        }

        // mandatory screenId.
        // this will be automatically filled by gameobject name on awake
        // this id will be used to identify the screen
        public string ScreenId { get; set; }

        // state of the screen visible or not
        // the status is handled by the screen itself so private set
        public bool IsVisible { get; private set; }

        /// <summary>
        /// When awake screenId is registred
        /// </summary>
        protected virtual void Awake()
        {
            ScreenId = gameObject.name;
        }

        /// <summary>
        /// Overridable unity start
        /// </summary>
        protected virtual void Start()
        {
            
        }

        /// <summary>
        /// When a screen is destroy the corresponding callback is called and all events are unset
        /// </summary>
        protected void OnDestroy()
        {
            OnScreenDestroyed?.Invoke(this);
            OnInTransitionFinished = null;
            OnOutTransitionFinished = null;
            OnCloseRequest = null;
            OnScreenDestroyed = null;
        }

        /// <summary>
        /// Show a screen
        /// </summary>
        /// <param name="parameters">The parameters that will be set</param>
        public void Show(IUIScreenParameters parameters = default(IUIScreenParameters))
        {
            // if there's parameters to set call else use default ones
            if (parameters != null)
            {
                if (parameters is TParameters screenParameters)
                {
                    SetParameters(screenParameters);
                }
                else
                {
                    Debug.Log("[UIScreenController] Invalid parameters");
                }
            }

            // you may adjust hierarchy when showed, default is no change
            AdjustHierarchyOnShow();
            
            // you may adjust things when parameters are set, default is nothing
            OnParametersSet();
            
            // if the gameobject is not yet active by itself
            if (!gameObject.activeSelf)
            {
                // play transition and inform that transition is done when done calling InTransitionDone
                PlayTransition(InTransition, InTransitionDone, true);
            }
            else
            {
                // else it is already visible so only call InTransitionDone
                InTransitionDone();
            }
        }
        

        /// <summary>
        /// This function is used to hide a screen (close the screen)
        /// It may play the Out transition or not depending of the given parameters
        /// Sometimes we may just want to hide the window with no animation
        /// </summary>
        /// <param name="animate">if true the out transition will be played, else no transition</param>
        public void Hide(bool animate = true)
        {
            // play or not the out transition and OutTransitionDone callback will be called after visibility set
            PlayTransition(animate ? OutTransition : null, OutTransitionDone, false);
        }
        

        /// <summary>
        /// This function plays a transition if needed and adjust screen visibility status
        /// It is called on In and Out transition, so the final status of the window to set is given in parameter
        /// </summary>
        /// <param name="transition">the transition to play (may be null)</param>
        /// <param name="onFinish">callback to call when transition is over</param>
        /// <param name="isVisible">screen visibility status to set upon completion</param>
        private void PlayTransition(UITransition transition, Action onFinish, bool isVisible)
        {
            // if there's no transition
            if (transition == null)
            {
                // just set the screen visibility status according to parameter
                gameObject.SetActive(isVisible);
                // and callback to say that transition is done
                onFinish?.Invoke();
            }
            else
            {
                // if the screen status is visible but the gameobject is not active by itself
                if (isVisible && !gameObject.activeSelf)
                {
                    // activate it
                    gameObject.SetActive(true);
                }
                
                // then play transition giving transition over callback
                transition.Play(transform, onFinish);
            }
        }

        /// <summary>
        /// This will be called when an in transition is over
        /// </summary>
        private void InTransitionDone()
        {
            // in transition so the status of the screen should be visible
            IsVisible = true;
            
            // callback registred object that the transition is over
            // actually WindowLayer use this to control when blocking/unblocking of ui should occurs (no raycast enable during transitions)
            OnInTransitionFinished?.Invoke(this);
        }

        /// <summary>
        /// This will be called when an out transition is over
        /// </summary>
        private void OutTransitionDone()
        {
            // out transition so screen status should be set to invisible
            IsVisible = false;
            // associated game object disabled
            gameObject.SetActive(false);
            // registred listener should be alerted
            // actually WindowLayer use this to control when blocking/unblocking of ui should occurs (no raycast enable during transitions)
            OnOutTransitionFinished?.Invoke(this);
        }


        /// <summary>
        /// Function called when parameters are really affected to screen when given as parameters to Show
        /// Could be overriden to do specific tasks at this moment
        /// </summary>
        /// <param name="uiScreenParameters"></param>
        protected virtual void SetParameters(TParameters uiScreenParameters)
        {
            m_Parameters = uiScreenParameters;
        }
        
        /// <summary>
        /// Function called after parameters are set
        /// Could be overriden to do specific task (maybe set some interface elements)
        /// </summary>
        protected virtual void OnParametersSet()
        {
            
        }
        
        /// <summary>
        /// This function is called when the screen will be shown (before transition)
        /// It could be override to adjust position in hierarchy (maybe set it top most)
        /// </summary>
        protected virtual void AdjustHierarchyOnShow()
        {
        }
    }
}
