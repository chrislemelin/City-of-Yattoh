using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour {

    public GameObject target = null;
    public Vector3 offest;
    float height = -1;

    private void Update()
    {
        if(Camera.main != null)
        {
            height = Camera.main.pixelHeight;
        }
        if(height != -1 && target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position) + offest * height;
        }

    }
}
