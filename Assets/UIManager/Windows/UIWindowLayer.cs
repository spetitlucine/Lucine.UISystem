using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    public class UIWindowLayer : UILayerController<IUIWindowController>
    {
        private HashSet<IUIScreenController> screensTransitioning;

        public IUIWindowController CurrentWindow { get; private set; }

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;

        private bool IsScreenTransitionInProgress
        {
            get { return screensTransitioning.Count != 0; }
        }

        public override void Initialize()
        {
            screensTransitioning = new HashSet<IUIScreenController>();
            base.Initialize();
        }

        protected override void ProcessScreenRegister(string screenId, IUIWindowController controller)
        {
            base.ProcessScreenRegister(screenId, controller);
            controller.OnInTransitionFinished += OnInAnimationFinished;
            controller.OnOutTransitionFinished += OnOutAnimationFinished;
            controller.OnCloseRequest += OnCloseRequestedByWindow;
        }

        protected override void ProcessScreenUnregister(string screenId, IUIWindowController controller)
        {
            base.ProcessScreenUnregister(screenId, controller);
            controller.OnInTransitionFinished -= OnInAnimationFinished;
            controller.OnOutTransitionFinished -= OnOutAnimationFinished;
            controller.OnCloseRequest -= OnCloseRequestedByWindow;
        }

        public override void ShowScreen(IUIWindowController screen)
        {
            ShowScreen<IUIWindowParameters>(screen, null);
        }

        public override void ShowScreen<TProp>(IUIWindowController screen, TProp properties)
        {
            IUIWindowParameters windowParameters = properties as IUIWindowParameters;

            DoShow(screen, windowParameters);
        }

        public override void HideScreen(IUIWindowController screen)
        {
            if (screen == CurrentWindow)
            {
                AddTransition(screen);
                screen.Hide();

                CurrentWindow = null;
            }
            else
            {
                Debug.LogError(
                    string.Format(
                        "[UIWindowLayer] Hide requested on WindowId {0} but that's not the currently open one ({1})! Ignoring request.",
                        screen.ScreenId, CurrentWindow != null ? CurrentWindow.ScreenId : "current is null"));
            }
        }

        public override void HideAll(bool shouldAnimateWhenHiding = true)
        {
            base.HideAll(shouldAnimateWhenHiding);
            CurrentWindow = null;
        }

        public override void ReparentScreen(IUIScreenController controller, Transform screenTransform)
        {
            IUIWindowController window = controller as IUIWindowController;

            if (window == null)
            {
                Debug.LogError("[UIWindowLayer] Screen " + screenTransform.name + " is not a Window!");
            }
            else
            {
                if (window.IsPopup)
                {

                    // make it foreground
                    return;
                }
            }

            base.ReparentScreen(controller, screenTransform);
        }

        private void DoShow(IUIWindowController screen, IUIWindowParameters parameters)
        {
            if (CurrentWindow != null
                && CurrentWindow.HideWhenFocusLost
                && !screen.IsPopup)
            {
                CurrentWindow.Hide();
            }

            AddTransition(screen);

            if (screen.IsPopup)
            {
                ShowDarkBackground(true);
            }

            screen.Show();

            CurrentWindow = screen;
        }


        private void OnCloseRequestedByWindow(IUIScreenController screen)
        {
            HideScreen(screen as IUIWindowController);
        }

        private void OnOutAnimationFinished(IUIScreenController screen)
        {
            RemoveTransition(screen);
            IUIWindowController window = screen as IUIWindowController;
            if (window.IsPopup)
            {
                ShowDarkBackground(false);
            }
        }

        private void OnInAnimationFinished(IUIScreenController screen)
        {
            RemoveTransition(screen);
        }

        private void AddTransition(IUIScreenController screen)
        {
            screensTransitioning.Add(screen);
            RequestScreenBlock?.Invoke();
        }

        private void RemoveTransition(IUIScreenController screen)
        {
            if(screensTransitioning.Contains(screen))
                screensTransitioning.Remove(screen);
            else
            {
                Debug.Log("[UIWindowLayer : try to remove a transition not started");
            }
            
            if (!IsScreenTransitionInProgress)
            {
                RequestScreenUnblock?.Invoke();
            }
        }

        private void ShowDarkBackground(bool show)
        {
            Debug.Log("Show dark background "+ show);
            
        }

        public bool IsVisible(string windowId)
        {
            return IsScreenVisibleById(windowId);
        }
    }
}