using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common.Utils
{
    public class CameraMove : MonoBehaviour
    {
        private static bool moving = false;
        public static bool Moving
        {
            get { return moving; }
        }

        public float xMin = -10, xMax = 100, yMin = -10, yMax = 100;

        private bool isPanning;
        private Vector3 origin;
        private Vector3 diff;


        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                moving = true;
                diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
                if (isPanning == false)
                {
                    isPanning = true;
                    origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                moving = false;
                isPanning = false;
            }
            if (isPanning == true)
            {
                Vector3 newPosition = origin - diff;
                if(newPosition.x < xMin)
                {
                    newPosition.x = xMin;
                }
                if(newPosition.x > xMax)
                {
                    newPosition.x = xMax;
                }
                if(newPosition.y < yMin)
                {
                    newPosition.y = yMin;
                }
                if(newPosition.y > yMax)
                {
                    newPosition.y = yMax;
                }

                Camera.main.transform.position = newPosition;
                
            }
        }
    }
}