using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Lucine.UISystem
{
   /// <summary>
   /// This class store the text id of the associated Unity Text object
   /// </summary>
   [RequireComponent(typeof(Text))]
    public class UIText : MonoBehaviour
    {
        [SerializeField]
        protected string m_TextId;

        private Text m_Text;

        /// <summary>
        /// Initialize associated Text component with the text which id is m_TextId;
        /// </summary>
        void Start()
        {
            m_Text = GetComponent<Text>();
            
            m_Text.text = TextManager.Instance.GetText(m_TextId);
        }
    }
}