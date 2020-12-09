using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Lucine.Helpers
{
    /// <summary>
    /// Dictionary are not serializable by default
    /// This class extends Dictionary and implements IXmlSerializable in order to do it
    /// </summary>
    /// <typeparam name="TKey">will be string</typeparam>
    /// <typeparam name="TValue">will be string</typeparam>
    [Serializable]
    public class Database<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if(reader.IsEmptyElement)
            {
                return;
            }

            reader.Read();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                object key = reader.GetAttribute("Id");
                object value = reader.GetAttribute("Text");
                this.Add((TKey)key, (TValue) value);
                reader.Read();
            }
        }
        
        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("Entry");
                writer.WriteAttributeString("Id",key.ToString());
                writer.WriteAttributeString("Text",this[key].ToString());
                writer.WriteEndElement();
            }
        }
    }
    
    
    /// <summary>
    /// This is the class containing the text database and can be serialized or deserialized
    /// </summary>
    [Serializable]
    public class TextDatabase
    {
        public Database<string,string> Texts = new Database<string, string>();

        public TextDatabase()
        {
        }
        
        /// <summary>
        /// Create a TextDatabase object from xml string
        /// </summary>
        /// <param name="xmlString">the xml string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Serialize a text dabatabase, not really used at the moment but to show a sample
        /// Could be used if we make a text editor and that the database could be saved
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public static string Serialize(TextDatabase database)
        {
            var serializer = new XmlSerializer(typeof(TextDatabase));

            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, database);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Get the string corresponding to the given ID
        /// It is case sensitive
        /// </summary>
        /// <param name="textId">ID of the text to get</param>
        /// <returns>The corresponding text or ID not found... if not found</returns>
        public string GetText(string textId)
        {
            string result = textId + " not found...";
            Texts.TryGetValue(textId, out result);
            return result;
        }

        /// <summary>
        /// Could be called from start to display an xml sample that can be base of work when no textdatabase available in project
        /// </summary>
        private void ShowXmlSample()
        {
            Texts.Add("ID1", "Texte id 1");
            Texts.Add("ID2", "Texte id 2");
            Debug.Log(TextDatabase.Serialize(this));
        }
    }
}