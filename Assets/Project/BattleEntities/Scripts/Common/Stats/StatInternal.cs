using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// for internal use for the stat module, use the Stat object for using stats 
/// anywhere outside of the stats namespace
/// </summary>
namespace Placeholdernamespace.Battle.Entities.AttributeStats.Internal
{

    [System.Serializable]
    public class StatInternal
    {
        private StatType type;
        public StatType Type
        {
            get {return type; }
        }

        [SerializeField]
        private float value;
        public float Value
        {
            get { return value; }
        }

        [SerializeField]
        private bool display = true;
        public bool Display
        {
            get { return display; }
        }

        public StatInternal(StatType type, float value)
        {
            this.type = type;
            this.value = value;
        }

        public StatInternal(StatType type, StatInternal stat)
        {
            this.type = type;
            this.value = stat.value;
        }
    }
}