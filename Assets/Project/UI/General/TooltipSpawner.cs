using Placeholdernamespace.Common.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Placeholdernamespace.Common.UI
{
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private float waitTime = .5f;

        private float enterTime;

        [SerializeField]
        private int fontSize = 12;

        [SerializeField]
        private GameObject tooltip;

        private bool placed = false;

        private Func<string> getDescription;
        private Func<string> getTitle;
        private Func<string> getFlavorText;

        private bool hover = false;
        public bool Hover
        {
            get { return hover; }
        }


        private GameObject spawnedTooltip = null;
        private RectTransform spawnedTooltipRect = null;

        private Action clickAction;

        // Use this for initialization
        void Start()
        {
            if(GetComponent<Button>() != null)
            {
                clickAction = () => GetComponent<Button>().onClick.Invoke();
            }
        }

        void Update()
        {
            if (spawnedTooltip == null
                && hover
                && (Time.time - enterTime)
                > waitTime
                && (getDescription != null || getTitle != null)
                && (getDescription() != null || getTitle() != null) )
            {
                spawnedTooltip = Instantiate(tooltip);
                spawnedTooltip.GetComponentInChildren<Tooltip>().Init(clickAction);
                spawnedTooltip.GetComponentInChildren<Tooltip>().setDescription(getDescription());
                spawnedTooltip.GetComponentInChildren<Tooltip>().setTitle(getTitle());
                spawnedTooltip.GetComponentInChildren<Tooltip>().setFlavorText(getFlavorText());
                spawnedTooltip.GetComponentInChildren<Tooltip>().setDescriptionFontSize(fontSize);
                spawnedTooltip.transform.SetParent(FindObjectOfType<Canvas>().transform,false);
                spawnedTooltip.transform.SetAsLastSibling();
                spawnedTooltipRect = spawnedTooltip.transform.GetChild(0).GetComponent<RectTransform>();
                spawnedTooltip.transform.position = new Vector3(10000, 10000);
                placed = false;
            }
            if(!placed && spawnedTooltip != null && spawnedTooltipRect.rect.height != 0)
            {
                Vector3 mousePos = Input.mousePosition;
                float x = 0;
                float width = spawnedTooltip.GetComponent<RectTransform>().rect.width * spawnedTooltip.GetComponent<RectTransform>().transform.lossyScale.x;
                float height = spawnedTooltipRect.rect.height;
                //float height = spawnedTooltipRect.GetChild(0).Gec
                float tipWidth = width - (spawnedTooltipRect.rect.width * spawnedTooltipRect.transform.lossyScale.x);
                

                if (mousePos.x > (Camera.main.pixelWidth/2))
                {
                    x = mousePos.x - width/2;
                }
                else
                {
                    float widthValue = width / 2 - tipWidth;
                    x = mousePos.x + (widthValue);
                }

                if(mousePos.y < Camera.main.pixelHeight/2)
                {
                    spawnedTooltip.GetComponent<RectTransform>().pivot =  new Vector2(.5f,0);
                }

                float y = mousePos.y - height / 2;                

                if (y - height / 2 < 0)
                {
                    y = height / 2;
                }

                spawnedTooltip.transform.position = new Vector3(x,mousePos.y);
                placed = true;
            }
            if(spawnedTooltip!= null && !hover && !spawnedTooltip.GetComponentInChildren<Tooltip>().Hover)
            {
                Destroy(spawnedTooltip);
            }
      
        }

        public void SetClickAction(Action clickAction)
        {
            this.clickAction = clickAction;
        }

        public void Init(Func<string> getTitle, Func<string> getDescription, Func<string> getFlavorText = null)
        {
            if (getFlavorText != null)
            {
                this.getFlavorText = getFlavorText;
            }
            else
            {
                this.getFlavorText = () => { return null; };
            }            
            this.getDescription = getDescription;          
            this.getTitle = getTitle;
        }

        public void OnDestroy()
        {
            Destroy(spawnedTooltip);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!hover)
            {
                enterTime = Time.time;
            }
            hover = true; 
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hover = false;
                       
        }



    }
}