using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// UIScreen controller is the base class for all types of windows, panels, popup whatever u want
    /// It implements the IUIScreenController interface
    /// </summary>
    /// <typeparam name="TParameters"></typeparam>
    public class UIScreenController<TParameters> : MonoBehaviour, IUIScreenController where TParameters : IUIScreenParameters
    {
        [SerializeField]
        private UITransition m_inTransition;
        private UITransition InTransition
        {
            get => m_inTransition;
            set => m_inTransition = value;
        }
        
        [SerializeField]
        private UITransition m_outTransition;
        private UITransition OutTransition
        {
            get => m_outTransition;
            set => m_outTransition = value;
        }

        public Action<IUIScreenController> OnInTransitionFinished { get; set; }
        public Action<IUIScreenController> OnOutTransitionFinished { get; set; }
        public Action<IUIScreenController> OnCloseRequest { get; set; }
        public Action<IUIScreenController> OnScreenDestroyed { get; set; }
        
        [SerializeField]
        private TParameters parameters;
        protected TParameters Parameters
        {
            get => parameters;
            set => parameters = value;
        }

        public string ScreenId { get; set; }
        
        public bool IsVisible { get; private set; }

        protected virtual void Awake()
        {
            ScreenId = gameObject.name;
        }

        protected virtual void Start()
        {
        }

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
        /// <param name="parameters"></param>
        public void Show(IUIScreenParameters parameters = default(IUIScreenParameters))
        {
            if (parameters != null)
            {
                if (parameters is TParameters)
                {
                    SetParameters((TParameters) parameters);
                }
                else
                {
                    Debug.Log("[UIScreenController] Invalid parameters");
                }
            }

            HierarchyFixOnShow();
            OnParametersSet();
            
            if (!gameObject.activeSelf)
            {
                PlayTransition(InTransition, InTransitionDone, true);
            }
            else
            {
                InTransitionDone();
            }
        }

        private void PlayTransition(UITransition transition, Action onFinish, bool isVisible)
        {
            if (transition == null)
            {
                gameObject.SetActive(isVisible);
                onFinish?.Invoke();
            }
            else
            {
                if (isVisible && !gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
                
                transition.Play(transform, onFinish);
            }
        }

        private void InTransitionDone()
        {
            IsVisible = true;
            
            OnInTransitionFinished?.Invoke(this);
        }

        private void OutTransitionDone()
        {
            IsVisible = false;
            gameObject.SetActive(false);
            OnOutTransitionFinished?.Invoke(this);
        }

        protected virtual void OnParametersSet()
        {
            
        }
        
        protected virtual void SetParameters(TParameters uiScreenParameters)
        {
            parameters = uiScreenParameters;
        }

        public void Hide(bool animate = true)
        {
            PlayTransition(animate ? OutTransition : null, OutTransitionDone, false);
        }
        
        protected virtual void HierarchyFixOnShow()
        {
        }
        

    }
   
}
