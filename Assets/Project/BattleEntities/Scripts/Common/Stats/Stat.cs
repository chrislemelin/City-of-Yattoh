using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Placeholdernamespace.Battle.Entities.AttributeStats.Internal;

/// <summary>
/// for EXTERNAL use of the stat module, why are there 2? because the value need to be an int for the final stat determination and needs to be a float
/// when calculating the modifiers and I dont trust myself to always remember to cast it when using it
/// </summary>
namespace Placeholdernamespace.Battle.Entities.AttributeStats
{
   


    [System.Serializable]
    public class Stat
    {
        private static Dictionary<StatType, string> toolTips = new Dictionary<StatType, string>()
        {
            { StatType.AP, "Used to perform actions, everyone gains 2 per turn" },
            { StatType.Armour, "Negates Physical Damage" },
            { StatType.Health, "Dies when this reaches zero" },
            { StatType.Inteligence, "LOL THIS ISNT BEING USED YET DELETE THIS" },
            { StatType.Movement, "Can spend one ap fill momement, spend one movement to move one square" },
            { StatType.Speed, "Determines who goes first, movement is the tie breaker" },
            { StatType.Strength, "Base damage that is dealt with skills" }
        };


        [SerializeField]
        private StatType type;
        public StatType Type
        {
            get { return type; }
        }

        [SerializeField]
        private int value;
        public int Value
        {
            get { return value; }
        }

        private bool display;
        public bool Display
        {
            get { return display; }
        }

        private string toolTip;
        public string ToolTip
        {
            get { return toolTip; }
        }

        public Stat(StatInternal statInternal)
        {
            type = statInternal.Type;
            display = statInternal.Display;
            // here to do rounding logic, right now rounding down is happening
            value = (int)statInternal.Value;
            toolTip = toolTips[type];
        }

        public Stat(Stat stat, int newValue)
        {
            type = stat.type;
            display = stat.display;
            value = newValue;
            toolTip = toolTips[type];
        }      

        public Stat(Stat stat)
        {
            type = stat.type;
            value = stat.value;
            display = stat.display;
            toolTip = toolTips[type];
        }
    }
}