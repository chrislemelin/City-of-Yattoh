using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour {

    public GameObject target;
    public Vector3 offest;

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position) + offest;

    }
}
