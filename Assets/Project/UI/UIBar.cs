using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placeholdernamespace.Battle.UI
{
    public class UIBar : MonoBehaviour
    {

        [SerializeField]
        private GameObject fill;

        private float value;

        public void SetValue(float value)
        {
            this.value = value;
            fill.transform.localScale = new Vector3(value, fill.transform.localScale.y, fill.transform.localScale.z);
        }

    }
}