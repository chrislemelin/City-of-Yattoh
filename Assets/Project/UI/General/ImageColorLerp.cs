using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Placeholdernamespace.Common.UI
{

    public class ImageColorLerp : MonoBehaviour
    {
        public Color primaryColor;
        public Color secondaryColor;

        private float currentLerp = 0;
        public float lerpValue = 1;

        [SerializeField]
        private Image image;
    
        // Update is called once per frame
        void Update()
        {
            image.color = Color.Lerp(primaryColor, secondaryColor, lerpValue);
        }

        public void setLerp(float newValue, float speed)
        {
            if(speed == -1)
            {
                currentLerp = newValue;
            }
            else
            {

            }
        }

    }
}
