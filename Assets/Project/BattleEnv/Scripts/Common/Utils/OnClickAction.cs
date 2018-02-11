using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickAction : MonoBehaviour, IPointerClickHandler{

    public Action clickAction;

    private void OnMouseUp()
    {
        if(clickAction != null)
        {
            clickAction();
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (clickAction != null)
        {
            clickAction();
        }
    }
}
