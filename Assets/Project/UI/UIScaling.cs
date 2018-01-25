using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaling : MonoBehaviour {

    private Vector3 scale = new Vector3(-.75f,.75f,1);

    public void Start()
    {
        transform.localScale = scale;
    }
}
