using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour {

    public GameObject target;
    public Vector3 offest;
    float height = -1;

    private void Update()
    {
        if(Camera.current != null)
        {
            height = Camera.current.pixelHeight;
        }
        if(height != -1)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position) + offest * height;
        }

    }
}
