using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Placeholdernamespace.Battle.Entities.AttributeStats
{
    [System.Serializable]
    public class Stat
    {

        [SerializeField]
        private string name;
        public string Name
        {
            get { return name; }
        }

        [SerializeField]
        private float value;
        public float Value
        {
            get { return value; }
        }

        public Stat(string name, float value)
        {
            this.name = name;
            this.value = value;
        }

    }
}