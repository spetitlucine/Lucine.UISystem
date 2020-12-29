using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Lucine.Helpers;
using NUnit.Framework.Constraints;
using Event = Lucine.Helpers.Event;

namespace Tests
{
    public class Helpers_Tests_Events
    {
        public class EventsNoParam : Lucine.Helpers.Event
        {
        };
        public int m_EventsNoParamCallNumber = 0;

        public class EventsOneParam : Lucine.Helpers.Event<int>
        {
        }
        public int m_EventsOneParamCallNumber = 0;
        
        // A Test behaves as an ordinary method
        [Test]
        public void Helpers_Tests_EventsSimplePasses()
        {
            // Use the Assert class to test conditions
            // Test events with no parameters
            m_EventsNoParamCallNumber = 0;
            Events.Instance.TypeOf<EventsNoParam>().AddListener(ListenerNoParam);
            Events.Instance.TypeOf<EventsNoParam>().Dispatch();
            Assert.AreEqual(m_EventsNoParamCallNumber,1);
            Events.Instance.TypeOf<EventsNoParam>().Dispatch();
            Assert.AreEqual(m_EventsNoParamCallNumber,2);
            Events.Instance.TypeOf<EventsNoParam>().RemoveListener(ListenerNoParam);
            Events.Instance.TypeOf<EventsNoParam>().Dispatch();
            Assert.AreEqual(m_EventsNoParamCallNumber,2);
            
            m_EventsOneParamCallNumber = 0;
            Events.Instance.TypeOf<EventsOneParam>().AddListener(ListenerOneParam);
            Events.Instance.TypeOf<EventsOneParam>().Dispatch(1);
            Assert.AreEqual(m_EventsOneParamCallNumber,1);
            Events.Instance.TypeOf<EventsOneParam>().Dispatch(2);
            Assert.AreEqual(m_EventsOneParamCallNumber,2);
            Events.Instance.TypeOf<EventsOneParam>().RemoveListener(ListenerOneParam);
            Events.Instance.TypeOf<EventsOneParam>().Dispatch(1);
            Assert.AreEqual(m_EventsNoParamCallNumber,2);
        }

        private void ListenerNoParam()
        {
            m_EventsNoParamCallNumber++;
        }
        
        private void ListenerOneParam(int param)
        {
            m_EventsOneParamCallNumber++;
            Assert.AreEqual(param,m_EventsOneParamCallNumber);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Helpers_Tests_EventsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
