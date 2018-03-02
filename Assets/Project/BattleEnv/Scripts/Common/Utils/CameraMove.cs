using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Common.Utils
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField]
        private float cameraSpeed = .01f;

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
            if (Input.GetMouseButton(2))
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


                Camera.main.transform.position = newPosition;
            }

            Vector3 position = Camera.main.transform.position;
            if (Input.GetKey(KeyCode.W))
                position += new Vector3(0, 1, 0) * cameraSpeed* Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                position += new Vector3(0, -1, 0) * cameraSpeed* Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                position += new Vector3(-1, 0, 0) * cameraSpeed* Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                position += new Vector3(1, 0, 0) * cameraSpeed* Time.deltaTime;
            if (position.x < xMin)
            {
                position.x = xMin;
            }
            if (position.x > xMax)
            {
                position.x = xMax;
            }
            if (position.y < yMin)
            {
                position.y = yMin;
            }
            if (position.y > yMax)
            {
                position.y = yMax;
            }

            Camera.main.transform.position = position;
        }



    }
}