using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common.Utils
{
    public class CameraMove : MonoBehaviour
    {
        private bool isPanning;
        private Vector3 origin;
        private Vector3 diff;


        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
                if (isPanning == false)
                {
                    isPanning = true;
                    origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                isPanning = false;
            }
            if (isPanning == true)
            {
                Camera.main.transform.position = origin - diff;
            }
        }
    }
}