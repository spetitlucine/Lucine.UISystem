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
        /// Initialize associated Text component
        /// Register to OnTextDatabaseRefreshed event
        /// </summary>
        void Start()
        {
            m_Text = GetComponent<Text>();
            
            Events.Instance.TypeOf<OnTextDatabaseChanged>().AddListener(OnTextChanged);
        }

        /// <summary>
        /// Change the text when event fired
        /// </summary>
        public void OnTextChanged()
        {
            m_Text.text = TextManager.Instance.GetText(m_TextId);
        }
        
        /// <summary>
        /// Remove listener on destroy
        /// </summary>
        void OnDestroy()
        {
            Events.Instance.TypeOf<OnTextDatabaseChanged>().RemoveListener(OnTextChanged);
            
        }
    }
}