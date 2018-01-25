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

        [SerializeField]
        private TextMeshProUGUI flavorText;

        private bool hover = false;
        public bool Hover
        {
            get { return hover; }
        }

        public void setFlavorText(string flavorText)
        {
            this.flavorText.text = flavorText;
            if (flavorText == null || flavorText == "")
                this.flavorText.gameObject.SetActive(false);
        }

        public void setTitle(string title)
        {
            this.title.text = title;
            if (title == null || title == "")
                this.title.gameObject.SetActive(false);
        }

        public void setDescription(string description)
        {
            this.description.text = description;
            if (description == null || description == "")
                this.description.gameObject.SetActive(false);
        }

        public void setDescriptionFontSize(int fontSize)
        {
            description.fontSize = fontSize;
            flavorText.fontSize = fontSize;
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