using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Placeholdernamespace.Common.UI
{
    public class OnPointerDownListener : MonoBehaviour, IPointerDownHandler
    {

        public event Action pressed;
        public bool active = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (pressed != null && active)
            {
                pressed();
            }
        }
    }
}
