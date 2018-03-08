using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickAction : MonoBehaviour, IPointerClickHandler{

    public List<Action> clickActions = new List<Action>();
    public bool active = false;

    private void OnMouseUp()
    {
       foreach(Action action in clickActions)
       {
           action();
       }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        foreach (Action action in clickActions)
        {
            action();
        }
    }
}
