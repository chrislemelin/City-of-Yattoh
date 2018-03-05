using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Placeholdernamespace.Common.UI
{

    public class TooltipSpawnerStatic : MonoBehaviour {

       
        public string title;
        [TextArea]
        public string description;
        [TextArea]
        public string flavorText;

        // Use this for initialization
        void Start() {
            GetComponent<TooltipSpawner>().Init(() => title, () => description, () => flavorText);
            if(GetComponent<OnPointerDownListener>() != null)
            {
                GetComponent<TooltipSpawner>().SetClickAction(() => GetComponent<OnPointerDownListener>().OnPointerDown(null) );
            }
        }

  
    }
}
