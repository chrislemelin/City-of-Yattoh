using Placeholdernamespace.Common.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Placeholdernamespace.Common.UI
{
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float waitTime = 2;

        private float enterTime;

        [SerializeField]
        private GameObject tooltip;

        private bool placed = false;

        private Func<string> getDescription;
        private Func<string> getTitle;

        private bool hover = false;
        public bool Hover
        {
            get { return hover; }
        }


        private GameObject spawnedTooltip = null;
        private RectTransform spawnedTooltipRect = null;

        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {
            if(spawnedTooltip == null && hover && (Time.time - enterTime) > waitTime && (getDescription() != null || getTitle() != null))
            {
                spawnedTooltip = Instantiate(tooltip);
                spawnedTooltip.GetComponent<Tooltip>().setDescription(getDescription());
                spawnedTooltip.GetComponent<Tooltip>().setTitle(getTitle());
                spawnedTooltip.transform.SetParent(FindObjectOfType<Canvas>().transform);
                spawnedTooltip.transform.SetAsLastSibling();
                spawnedTooltipRect = spawnedTooltip.GetComponent<RectTransform>();
                spawnedTooltip.transform.position = new Vector3(10000, 10000);
                placed = false;
            }
            if(!placed && spawnedTooltip != null && spawnedTooltipRect.rect.width != 0)
            {
                Vector3 mousePos = Input.mousePosition;
                float x = mousePos.x - (spawnedTooltipRect.rect.width / 2);
                float y = mousePos.y - spawnedTooltipRect.rect.height / 2;
                spawnedTooltip.transform.position = new Vector3(x, y);
                placed = true;
            }
            if(spawnedTooltip!= null && !hover && !spawnedTooltip.GetComponent<Tooltip>().Hover)
            {
                Destroy(spawnedTooltip);
            }
      
        }

        public void Init(Func<string> getTitle, Func<string> getDescription)
        {
            this.getDescription = getDescription;
            this.getTitle = getTitle;
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