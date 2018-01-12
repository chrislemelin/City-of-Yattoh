using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Placeholdernamespace.Common.UI
{

    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI description;

        private bool hover = false;
        public bool Hover
        {
            get { return hover; }
        }


        public void setTitle(string title)
        {
            this.title.text = title;
        }

        public void setDescription(string description)
        {
            this.description.text = description;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;
        }

    }
}