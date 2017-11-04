using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnClickRedirect : MonoBehaviour {

    public abstract IClickable GetTarget();  

    public void OnMouseDown()
    {
        GetTarget().OnMouseDown();
    }
}
