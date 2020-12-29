using System.Collections;
using System.Collections.Generic;
using Lucine.UISystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UISystem_Tests_Screen
    {
        private bool m_ScreenDestroyed = false;
        private bool m_InTransitionFinished = false;
        private bool m_OutTransitionFinished = false;
        private bool m_OnCloseRequest = false;

        // A Test behaves as an ordinary method
        [Test]
        public void UISystem_Tests_ScreenSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator UISystem_Tests_ScreenWithEnumeratorPasses()
        {
            const string screenName = "TestScreen";
            GameObject go = new GameObject {name = screenName};
            ScreenTest testScreen = go.AddComponent<ScreenTest>();
            testScreen.OnScreenDestroyed += delegate(IUIScreenController controller)
            {
                m_ScreenDestroyed = true;
            };
            
            testScreen.OnInTransitionFinished += delegate(IUIScreenController controller)
            {
                m_InTransitionFinished = true;
            };
            
            testScreen.OnOutTransitionFinished += delegate(IUIScreenController controller)
            {
                m_OutTransitionFinished = true;
            };
            
            testScreen.OnCloseRequest += delegate(IUIScreenController controller)
            {
                m_OnCloseRequest = true;
            };
            
            yield return null;
            Assert.AreEqual(testScreen.ScreenId,screenName);
            Assert.AreEqual(testScreen.IsVisible,false);
            testScreen.Show();
            Assert.AreEqual(testScreen.IsVisible,true);
            Assert.AreEqual(m_InTransitionFinished,true);
            testScreen.Close();
            Assert.AreEqual(testScreen.IsVisible,false);
            Assert.AreEqual(m_OutTransitionFinished,true);
            Assert.AreEqual(m_OnCloseRequest,true);
            
            GameObject.Destroy(testScreen);
            yield return null;
            Assert.AreEqual(m_ScreenDestroyed,true);
            
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
