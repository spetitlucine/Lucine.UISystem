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
    [Serializable]
    [XmlRoot("Texts")]
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
    
    [Serializable]
    public class TextDatabase
    {
        public Database<string,string> Texts = new Database<string, string>();

        public TextDatabase()
        {
            Texts.Add("ID1", "Texte id 1");
            Texts.Add("ID2", "Texte id 2");
            Debug.Log(TextDatabase.Serialize(this));
        }
        
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
                return writer.ToString();
            }
        }

        public string GetText(string textId)
        {
            string result = textId + " not found...";
            Texts.TryGetValue(textId, out result);
            return result;
        }
    }
}