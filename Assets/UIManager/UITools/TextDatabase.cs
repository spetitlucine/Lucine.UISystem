using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Lucine.Helpers
{
    public class TextDatabase : MonoBehaviour
    {
        public Dictionary<string, string> m_TextDatabase = new Dictionary<string, string>();
        
        public static TextDatabase Deserialize(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(TextDatabase));
            TextDatabase result = new TextDatabase();
            
            using(TextReader reader = new StringReader(xmlString))
            {
                result = serializer.Deserialize(reader) as TextDatabase;
            }

            return result;
        }

        public static string Serialize(TextDatabase database)
        {
            var serializer = new XmlSerializer(typeof(TextDatabase));

            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, database);
                return serializer.ToString();
            }
        }

        public string GetText(string textId)
        {
            string result = textId + " not found...";
            m_TextDatabase.TryGetValue(textId, out result);
            return result;
        }
    }
}