using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Lucine.Helpers
{
    /// <summary>
    /// You can register to this event to be notified when database has been refreshed
    /// </summary>
    public class OnTextDatabaseChanged : Event { }

    public class TextManager : Singleton<TextManager>
    {
        /// <summary>
        /// Enumeration of location for TextDatabase
        /// </summary>
        public enum FileSource
        {
            StreamingAssets,
            Resources
        };
        
        // Where is located the text database
        [SerializeField]
        protected FileSource m_Source;
        // what is its name (no extension if in resources)
        [SerializeField]
        protected string m_TextDatabaseName;
        // init at awake
        [SerializeField]
        protected bool m_InitOnAwake = false;
        


        /// <summary>
        /// The text database that will be filled !
        /// </summary>
        private TextDatabase m_TextDatabase = new TextDatabase();

        /// <summary>
        /// On awake the database is loaded from the specified source
        /// You can also call LoadDatabase manually if you want to change the database
        /// </summary>
        void Awake()
        {
            if (m_InitOnAwake)
            {
                LoadDatabase(m_Source, m_TextDatabaseName);
            }
        }
        
        /// <summary>
        /// Load database from a string
        /// </summary>
        /// <param name="xmlString">The string from which to load xml</param>
        public void LoadFromString(string xmlString)
        {
            m_TextDatabase = TextDatabase.Deserialize(xmlString);
            
            // dispatch information that database is refreshed
            Events.Instance.TypeOf<OnTextDatabaseChanged>().Dispatch();
        }

        /// <summary>
        /// Load database from resources
        /// </summary>
        /// <param name="databaseName"></param>
        public void LoadFromResources(string databaseName)
        {
            TextAsset database = Resources.Load(databaseName) as TextAsset;
            string xmlDatabase = database.text;
            
            LoadFromString(xmlDatabase);
        }
        
        /// <summary>
        /// Load database from StreamingAssets
        /// </summary>
        /// <param name="databaseName"></param>
        public void LoadFromStreamingAssets(string databaseName)
        {
            string url = "file:///" + Application.streamingAssetsPath + "/" + databaseName;

            StartCoroutine(LoadItAsync(url));
        }

        /// <summary>
        /// really load the file async
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IEnumerator LoadItAsync(string filePath)
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log("Error: " + www.error);
            }
            else
            {
                string xmlDatabase = www.downloadHandler.text;
                Debug.Log("Received: " + xmlDatabase);
                LoadFromString(xmlDatabase);
            }
        }

        /// <summary>
        /// Load the text database with the given name from the given source
        /// </summary>
        /// <param name="source">source of the text databse</param>
        /// <param name="databaseName">name of the string database</param>
        /// <returns></returns>
        public void LoadDatabase(FileSource source, string databaseName)
        {
            switch (source)
            {
                case FileSource.Resources:
                    LoadFromResources(databaseName);
                    break;
                case FileSource.StreamingAssets:
                    LoadFromStreamingAssets(databaseName);
                    break;
            }
        }

        /// <summary>
        /// This function return the text corresponding to the specified id.
        /// </summary>
        /// <param name="textId">the id to retrieve</param>
        /// <returns>The text corresponding to the id</returns>
        public string GetText(string textId)
        {
            return m_TextDatabase.GetText(textId);
        }
    }
}