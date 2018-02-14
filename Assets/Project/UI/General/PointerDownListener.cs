using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Placeholdernamespace.Common.UI
{
    public class PointerDownListener : MonoBehaviour, IPointerDownHandler
    {

        public event Action pressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (pressed != null)
            {
                pressed();
            }
        }
    }
}
