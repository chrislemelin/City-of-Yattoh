using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueOption : IClickable{

    public string displayText;    
	public delegate void onOptionChoose();
	public onOptionChoose onChoose;

    public void OnMouseDown()
    {
        if(onChoose != null)
            onChoose();
    }
}
