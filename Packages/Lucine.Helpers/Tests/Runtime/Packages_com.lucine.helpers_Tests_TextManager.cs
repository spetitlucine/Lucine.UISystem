using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Lucine.Helpers;
using NUnit.Framework.Constraints;
using UnityEngine.UI;
using Event = Lucine.Helpers.Event;

namespace Tests
{
    public class Helpers_Tests_TextManager
    {
        private string xmlString = "";
        private bool xmlLoadedNotified = false;
        
        // A Test behaves as an ordinary method
        [Test]
        public void Helpers_Tests_TextManagerSimplePasses()
        {
            xmlString = "";
            xmlString += "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
            xmlString += "<TextDatabase xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";
            xmlString += "<Texts>";
            xmlString += "<Entry Id=\"ID_1\" Text=\"ID1\" />";
            xmlString += "<Entry Id=\"ID_2\" Text=\"ID2\" />";
            xmlString += "</Texts>";
            xmlString += "</TextDatabase>";
            
            xmlLoadedNotified = false;

            Events.Instance.TypeOf<OnTextDatabaseChanged>().AddListener(DatabaseLoaded);
            
            // Use the Assert class to test conditions
            TextManager.Instance.LoadFromString(xmlString);
            
            Assert.AreEqual(xmlLoadedNotified,true);
            
            Assert.AreEqual(TextManager.Instance.GetText("ID_1"),"ID1");
            Assert.AreEqual(TextManager.Instance.GetText("ID_2"),"ID2");
            Assert.AreEqual(TextManager.Instance.GetText("ID_3"),"ID_3 not found...");
            
            Events.Instance.TypeOf<OnTextDatabaseChanged>().RemoveListener(DatabaseLoaded);
        }

        private void DatabaseLoaded()
        {
            xmlLoadedNotified = true;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Helpers_Tests_TextManagerWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
