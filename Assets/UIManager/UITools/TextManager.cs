using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Lucine.Helpers
{
    public class TextManager : Singleton<TextManager>
    {
        public enum FileSource
        {
            StreamingAsset,
            Resources
        };
        
        [SerializeField]
        protected FileSource m_Source;
        [SerializeField]
        protected string m_TextDatabaseName;


        private TextDatabase m_TextDatabase = new TextDatabase();

        void Start()
        {
            LoadDatabase(m_Source,m_TextDatabaseName);
        }
        
        
        
        public void LoadFromResources(string databaseName)
        {
            TextAsset database = Resources.Load(databaseName) as TextAsset;
            string xmlDatabase = database.text;

            m_TextDatabase = TextDatabase.Deserialize(xmlDatabase);
        }

        public void LoadFromStreamingAssets(string databaseName)
        {
            /*
            string url = Application.streamingAssetsPath + "/" + databaseName;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();
                
                if(webRequest.)
            }
            
            HttpWebRequest req = HttpWebRequest(url);
            */
            
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
                case FileSource.StreamingAsset:
                    LoadFromStreamingAssets(databaseName);
                    break;
            }
        }

        public string GetText(string textId)
        {
            return m_TextDatabase.GetText(textId);
        }
    }
}